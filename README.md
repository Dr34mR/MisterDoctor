# Mister Doctor Twitch Bot

Mister Doctor is a modular twitch bot built to be extensible with each individual component able to accept a message and reply with a message where desired.

Mister Doctor started off as a single bot (substitution) and grew to encompass other bot functionalities as they became desired.

![Screenshot](FormMain.png?raw=true "Screenshot")

Things Mister Doctor does:
- **Random noun substitution**. If a noun is found and a % chance is met, a random noun will be replaced with a word or series of words in a wordlist that you set. Like [buttsbot](http://twitch.tv/buttsbot) but more flexible and with higher accuracy of appropriate substitution. Noun list is able to be expanded to or retracted from as required. Global cooldown between messages applies.

     - ***User*** - I'm going to grab a can of drink
     - ***Bot*** - I'm going to grab a xxxx of drink
     
- **Phrases.** If a message contains a particular word you set, a desired response you set will be sent. Responses can contain Emoji or be prefexed with slash commands like '/me'

     - ***User*** - Have you heard from joe latley?
     - ***Bot*** - Joe Momma!
     
- **Wildcards.** If a message reply contains one of the following wildcards it will be replaced accordingly.
     
     - ***$user*** - Will be replaced with the user who the bot is replying to
     - ***$channel*** - Will be replaved with the streamers channel name
     - ***$bot*** - Will be replaced with the bots username
     - ***$time*** - Will be replaced with the bots current local time (h:mm AM/PM)
     - ***$day*** - Will be replaved with the bots local date (d/mm/yyyy or mm/d/yyyy depending on your pc date)
     
- **Ability to Ignore users.** Great so bots don't chat with each other and so users who do not like the bot will not have their messages responded to.

- **Punctuation Awareness.** Words! With, punctuation. Are? Still considered. For both! Phrases, and Substitution.

- **GoodBot Badbot Responses.** 

     - If a user writes 'botname' + a goodbot word (yes, goodbot, plz, nice) will respond with a custom message you define. 
	 - If a user writes 'botname' + a badbot word (no, badbot, why, bad) will respond with a custom message you define.
	 
- **Settings.** Configurable ignore and unignore commands as well as max message words to ensure large messages are not spammed.

[Download Page](https://github.com/Dr34mR/MisterDoctor/releases)

### Packages currently being used
- TwitchLib.Client
- LiteDb
- Costura.Fody
