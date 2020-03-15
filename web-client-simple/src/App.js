import './App.css';
import {
  Components,
  createDirectLine,
  createCognitiveServicesSpeechServicesPonyfillFactory
} from 'botframework-webchat';
import React, { Component } from 'react';

import CustomDictationInterims from './CustomDictationInterims';
import CustomMicrophoneButton from './CustomMicrophoneButton';
import {
  region as fetchSpeechServicesRegion,
  token as fetchSpeechServicesToken
} from './fetchSpeechServicesCredentials';
import LastBotActivity from './LastBotActivity';

const { Composer } = Components;

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      directLine: null,
      webSpeechPonyfillFactory: null
    };
  }

  async componentDidMount() {
    const res = await fetch('https://circledemo-speech.azurewebsites.net/api/token/directline', { method: 'POST' });
    const { token } = await res.json();
    
    var authorizationToken = await fetchSpeechServicesToken();
    var region = await fetchSpeechServicesRegion();
    const webSpeechPonyfillFactory = await createCognitiveServicesSpeechServicesPonyfillFactory({
      authorizationToken: authorizationToken,
      region: region
    });

    this.setState(() => ({
      directLine: createDirectLine({
        token
      }),
      webSpeechPonyfillFactory
    }));
  }

  render() {
    const {
      state: { directLine, webSpeechPonyfillFactory }
    } = this;

    return (
      !!directLine &&
      !!webSpeechPonyfillFactory && (
        <Composer directLine={directLine} webSpeechPonyfillFactory={webSpeechPonyfillFactory}>
          <div className="App">
            <header className="App-header">
              <CustomMicrophoneButton className="App-speech-button" />
              <CustomDictationInterims className="App-speech-interims" />
              <LastBotActivity className="App-bot-activity" />
            </header>
          </div>
        </Composer>
      )
    );
  }
}
