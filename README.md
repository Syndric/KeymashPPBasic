# KeymashPPBasic
Calculates star ratings for Keymash PP

### How to run
1. Drag KeymashBasic to your Desktop
2. Navigate to "KeymashPPBasic/KeymashPPBasic". From there, do "dotnet run". You're in the right place if you see a .csproj file
3. Use "help" to see commands. Pull texts if you want to update them or you don't have them. Use fcrating to calculate the ratings.

### KeymashBasic
If you want to add your own quotes, copy the format of a valid quote in textContents such as textID 2. Assign a separate textID and file name.

All calculations will be found in textRatings.

If you delete combos.json, it will break.

###
A litte bit more about the combo list:
Basically, the combo list is a frequency list of letter combos, up to five letters long.  It ignores all punctuation, spacing, and is case sensitive.
A word such as "the" will appear in the combo list as "the".
A word such as "however" will appear in the combo list as "howev", "oweve", "wever"
The combos were grabbed from books from the Gutenberg project, which were downloaded randomly.
I made sure that they were all english. If you'd like to download some books for yourself, the
API call is 

  https://www.gutenberg.org/cache/epub/{num}/pg{num}.txt 
  
where num is 1-70000.
