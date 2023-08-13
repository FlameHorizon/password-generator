using static System.Console;

WriteLine("Provide length");
int len = int.Parse(ReadLine());

WriteLine($"DEBUG: Length is {len}");

WriteLine(@"Provide character set:
		1. lower letters
		2. capital letters
		3. numbers
		4. symbols");
	
string? userInput = ReadLine();

while (userInput is null)
{
	WriteLine("Incorrect input. Please provide character set.");
	userInput = ReadLine();
}

CharacterSets charSet = ParseCharacterSet(userInput);

CharacterSets ParseCharacterSet(string input)
{
	CharacterSets set = CharacterSets.None;
	var charCharacterSetMap = new Dictionary<char, CharacterSets>()
	{
		{ '1', CharacterSets.LowerLetters },
		{ '2', CharacterSets.CapitalLetters },
		{ '3', CharacterSets.Numbers},
		{ '4', CharacterSets.Symbols}
	};

	foreach (char chr in input)
	{
		if (charCharacterSetMap.ContainsKey(chr) == false)
		{
			WriteLine($"Value {chr} is not handled and is ignored.");
		}
		else 
		{
			set ^= charCharacterSetMap[chr];
		}
	}

	return set;
}

WriteLine($"Character set is {charSet}");
char response = 'Y';

while (response == 'Y')
{
	string password = GeneratePassword(len, charSet);

	if (password == string.Empty)
	{
		WriteLine("Password could not be generated. Exiting.");
		return;
	}

	WriteLine($"Generated password is: {password}");
	
	WriteLine("Would you like to regenerate password? (Y/N)");
	response = ReadKey().KeyChar;
}

string GeneratePassword(int passwordLength, CharacterSets charSet)
{
	const string lowerLetters = "abcdefghijklmnopqrstuvxyz";
	const string capitalLettes = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
	const string numbers = "0123456789";
	const string symbols = """!"#$%'()*+,-./:;<=>?@[]^+`{}""";

	if (charSet is CharacterSets.None)
	{
		WriteLine("Character set is none. Can't generate a password.");
		return string.Empty;
	}

	// Note, this method will not ensure that character
	// from the required character set will be used.
	// It will be just added to the pool of available characters.
	
	string characterPool = string.Empty;

	var characterSetMap = new Dictionary<CharacterSets, string>()
	{
		{ CharacterSets.LowerLetters, lowerLetters },
			{ CharacterSets.CapitalLetters, capitalLettes },
			{ CharacterSets.Numbers, numbers },
			{ CharacterSets.Symbols, symbols }
	};

	var enumerable = (Enum.GetValues(typeof(CharacterSets))) as IEnumerable<CharacterSets>;

	foreach (CharacterSets set in enumerable.Where(x => x != CharacterSets.None))
	{	
		if (charSet.HasFlag(set))
		{
			characterPool += characterSetMap[set];
		}
	}

	string password = string.Empty;
	while (password.Length < passwordLength)
	{
		password += characterPool[Random.Shared.Next(0, characterPool.Length)];
	}

	while (true)
	{
		if (charSet.HasFlag(CharacterSets.LowerLetters) && lowerLetters.Any(password.Contains) == false)
		{
			int i = Random.Shared.Next(0, password.Length);
			int j = Random.Shared.Next(0, lowerLetters.Length);

			password = password.Remove(i, 1).Insert(i, Convert.ToString((lowerLetters[j])));
			continue;
		}
	
		if (charSet.HasFlag(CharacterSets.CapitalLetters) && capitalLettes.Any(password.Contains) == false)
		{
			int i = Random.Shared.Next(0, password.Length);
			int j = Random.Shared.Next(0, capitalLettes.Length);

			password = password.Remove(i, 1).Insert(i, Convert.ToString((capitalLettes[j])));
			continue;
		}

		if (charSet.HasFlag(CharacterSets.Numbers) && numbers.Any(password.Contains) == false)
		{
			int i = Random.Shared.Next(0, password.Length);
			int j = Random.Shared.Next(0, numbers.Length);

			password = password.Remove(i, 1).Insert(i, Convert.ToString((numbers[j])));
			continue;
		}
		
		if (charSet.HasFlag(CharacterSets.Symbols) && symbols.Any(password.Contains) == false)
		{
			int i = Random.Shared.Next(0, password.Length);
			int j = Random.Shared.Next(0, symbols.Length);	

			password = password.Remove(i, 1).Insert(i, Convert.ToString((symbols[j])));
			continue;
		}

		break;	
	}

	System.Console.WriteLine($"Now password is {password}");
	return password;
}


[Flags]
enum CharacterSets {
	None = 0,
	LowerLetters = 1,
	CapitalLetters = 2,
	Numbers = 4,
	Symbols = 8
}
