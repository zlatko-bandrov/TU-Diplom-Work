# LottoDemo

This is my diploma work for Technical University Sofia, speciality Computer Systems and Technologies. The task which I implement was to create a infrastructure which serves an online lottery generator for different lottery games. The main technologies which I used are ASP.NET MVC 5.2, SQL Server 2014, Web API 2, C#, multi-threading. 

The full assignment is here:

Design a .NET solution representing a minified version of lottery workflow, namely:
	•	A lottery number generator service, which will output 6 (out of 49) numbers every 20 seconds
	•	A console application, which will read user ticket input –a username and six non repeating numbers, satisfying 0 < n <= 49
	•	A module or service which will compute the jackpot and winnings of the lottery every 10 seconds. 
	•	All inputs cost 1 unit which should be accumulated in the jackpot if the user doesn’t win after each draw.
	•	Set the initial Jackpot to 100000 units; single ticket price x = 1. By default each user starts with 10 units.
	•	Define the winnings as :
		o	3 matched numbers – 10
		o	4 – 100
		o	5 – 10000 
		o	6 - Jackpot 
	•	Keep user and jackpot balance durable in database and display them on input console app startup.
	•	Each user may purchase many tickets in a single draw
	•	Keep history of all user lines and draw numbers in database
