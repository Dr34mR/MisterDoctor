# Mister Doctor Twitch Bot

Mister Doctor is a modular twitch bot built to be extensible with each individual component able to accept a message and reply with a message where desired.

Mister Doctor started off as a single bot (substitution) and grew to encompass other bot functionalities as they became desired.

Things Mister Doctor does:
- Random noun substitution. If a noun is found and a % chance is met, a random noun will be replaced with a word in a wordlist that you set. Like [buttsbot](twitch.tv/buttsbot) but more flexible. Noun list is able to be expanded to or retracted from as required.

     - User - I'm going to grab a can of drink
     - Bot - I'm going to grab a xxxx of drink
     
- Phrases. If a message contains a particular word you set, a desired response you set will be sent. Responses can contain Emoji or be prefexed with slash commands like '/me'

     - User - Have you heard from joe latley?
     - Bot - Joe Momma!
     
- Ability to Ignore users. Great so bots don't chat with each other and so users who do not like the bot will not have their messages responded to.

![Screenshot](FormMain.png?raw=true "Screenshot")

[Download Page](https://github.com/Dr34mR/MisterDoctor/releases)

### Packages currently being used
- TwitchLib.Client
- LiteDb
- Costura.Fody
