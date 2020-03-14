export default async function() {
  const res = await fetch('https://circledemo-speech.azurewebsites.net/api/token/directline', { method: 'POST' });
  const { token } = await res.json();

  return token;
}
