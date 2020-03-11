﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Helpers;
using CoreBot.MockData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly CircleIntentRecognizer _luisRecognizer;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(CircleIntentRecognizer luisRecognizer, BookingDialog bookingDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(bookingDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var showActivitiesAsCards = true;

            if (!_luisRecognizer.IsConfigured)
            {
                // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            }

            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var luisResult = await _luisRecognizer.RecognizeAsync<CircleIntent>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case CircleIntent.Intent.ActivitiesEnquiry:
                    if (luisResult.Entities?.ActivityTime?.Length > 0)
                    {
                        List<string> activityNames =
                            Activities.GetActivityNamesByTimeEntityFilter(luisResult.Entities.ActivityTime.First())
                                .ToList();

                        if (showActivitiesAsCards)
                        {
                            var activitiesEnquiryText = "I found the following activities. Let me know if you would like to be booked on any of these.";
                            var enquiryMessageSpeak = VoiceMessageHelpers.WrapMessageInVoice(activitiesEnquiryText);
                            var activitiesEnquiryMessage = MessageFactory.Text(activitiesEnquiryText,
                                enquiryMessageSpeak, InputHints.IgnoringInput);
                            await stepContext.Context.SendActivityAsync(activitiesEnquiryMessage, cancellationToken);

                            foreach (var activityName in activityNames)
                            {
                                var attachments = new List<Attachment>();
                                var cardReply = MessageFactory.Attachment(attachments);

                                if (activityName.Contains("aquarium", StringComparison.InvariantCultureIgnoreCase)) cardReply.Attachments.Add(Cards.GetActivityCard(Activities.AquariumActivityId).ToAttachment());
                                else if (activityName.Contains("bus tour", StringComparison.InvariantCultureIgnoreCase)) cardReply.Attachments.Add(Cards.GetActivityCard(Activities.BusTourActivityId).ToAttachment());
                                else if (activityName.Contains("choir", StringComparison.InvariantCultureIgnoreCase)) cardReply.Attachments.Add(Cards.GetActivityCard(Activities.ChoirShowActivityId).ToAttachment());
                                else if (activityName.Contains("dinner", StringComparison.InvariantCultureIgnoreCase)) cardReply.Attachments.Add(Cards.GetActivityCard(Activities.DinnerActivityId).ToAttachment());

                                await stepContext.Context.SendActivityAsync(cardReply, cancellationToken);
                            }
                        }
                        else
                        {
                            var activitiesEnquiryText = Activities.BuildActivitiesString(activityNames);
                            var enquiryMessageSpeak = VoiceMessageHelpers.WrapMessageInVoice(activitiesEnquiryText);
                            var activitiesEnquiryMessage = MessageFactory.Text(activitiesEnquiryText,
                                enquiryMessageSpeak, InputHints.IgnoringInput);
                            await stepContext.Context.SendActivityAsync(activitiesEnquiryMessage, cancellationToken);
                        }
                    }
                    else
                    {
                        var activitiesEnquiryText =
                            "There are some interesting activities coming up! Give us a call at the front desk and we will help find something to suit you.";
                        var enquiryMessageSpeak = VoiceMessageHelpers.WrapMessageInVoice(activitiesEnquiryText);
                        var activitiesEnquiryMessage = MessageFactory.Text(activitiesEnquiryText,
                            enquiryMessageSpeak, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(activitiesEnquiryMessage, cancellationToken);
                    }

                    break;


                case CircleIntent.Intent.ActivityBooking:
                    var activityBookingText = "No problem! I've sent that through to the front desk and they'll be in touch soon with details.";

                    if (luisResult.Entities?.ActivityName?.Length > 0)
                    {
                        var activityName = luisResult.Entities.ActivityName.First();
                        activityBookingText = $"No problem! I've let the front desk know that you are interested in the {activityName}. They'll be in touch soon with details.";
                    }

                    var activityBookingSpeak = VoiceMessageHelpers.WrapMessageInVoice(activityBookingText);
                    var activityBookingMessage = MessageFactory.Text(activityBookingText, activityBookingSpeak, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(activityBookingMessage, cancellationToken);
                    break;

                case CircleIntent.Intent.FinishConversation:
                    var finishConversationText = $"Okay! I'm here if you need me for anything else.";
                    var finishConversationSpeak = VoiceMessageHelpers.WrapMessageInVoice(finishConversationText);
                    var finishConversationMessage = MessageFactory.Text(finishConversationText, finishConversationSpeak, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(finishConversationMessage, cancellationToken);
                    break;

                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessageSpeak = VoiceMessageHelpers.WrapMessageInVoice(didntUnderstandMessageText);
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageSpeak, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is BookingDetails result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                var messageSpeak = VoiceMessageHelpers.WrapMessageInVoice(messageText);
                var message = MessageFactory.Text(messageText, messageSpeak, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
