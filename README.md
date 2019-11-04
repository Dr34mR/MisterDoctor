# Twitch Substitution Bot
Sort of like buttsbot but single channel use and allows custom defined words and/or phrases.

![Screenshot](FormMain.png?raw=true "Screenshot")

Twitch substitution bot, once connected, will read messages in your channel and every so oftern replace a nominated word in a users message with a 'substitution word' or 'substitution phrase'.

Messages are parsed using the 'nounlist.txt' for a word to replace.
Messages that are single words are skipped regardless of noun match.

### Packages currently being used
- TwitchLib.Client
- LiteDb
- Costura.Fody
