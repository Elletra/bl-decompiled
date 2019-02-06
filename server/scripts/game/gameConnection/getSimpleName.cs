function GameConnection::getSimpleName ( %client )
{
	if ( GameWindowExists()  &&  !$Server::Dedicated )
	{
		return %client.getPlayerName();
	}

	if ( %client.simpleName !$= "" )
	{
		return %client.simpleName;
	}

	%simpleName = %client.getPlayerName();

	%simpleName = strreplace (%simpleName, "\xa1", "I");
	%simpleName = strreplace (%simpleName, "\xa2", "C");
	%simpleName = strreplace (%simpleName, "\xa3", "L");
	%simpleName = strreplace (%simpleName, "\xa4", "O");
	%simpleName = strreplace (%simpleName, "\xa5", "Y");
	%simpleName = strreplace (%simpleName, "\xa6", "I");
	%simpleName = strreplace (%simpleName, "\xa7", "S");
	%simpleName = strreplace (%simpleName, "\xa9", "C");
	%simpleName = strreplace (%simpleName, "\xaa", "A");
	%simpleName = strreplace (%simpleName, "\xab", "<");
	%simpleName = strreplace (%simpleName, "\xac", "-");
	%simpleName = strreplace (%simpleName, "\xae", "R");
	%simpleName = strreplace (%simpleName, "\xaf", "-");
	%simpleName = strreplace (%simpleName, "\xb0", "O");
	%simpleName = strreplace (%simpleName, "\xb1", "+");
	%simpleName = strreplace (%simpleName, "\xb2", 2);
	%simpleName = strreplace (%simpleName, "\xb3", 3);
	%simpleName = strreplace (%simpleName, "\xb5", "U");
	%simpleName = strreplace (%simpleName, "\xb6", "P");
	%simpleName = strreplace (%simpleName, "\xb7", "");
	%simpleName = strreplace (%simpleName, "\xb8", "");
	%simpleName = strreplace (%simpleName, "\xb9", 1);
	%simpleName = strreplace (%simpleName, "\xba", "O");
	%simpleName = strreplace (%simpleName, "\xbb", ">");
	%simpleName = strreplace (%simpleName, "\xbc", "1/4");
	%simpleName = strreplace (%simpleName, "\xbd", "1/2");
	%simpleName = strreplace (%simpleName, "\xbe", "3/4");
	%simpleName = strreplace (%simpleName, "\xde", "P");
	%simpleName = strreplace (%simpleName, "\xd7", "X");
	%simpleName = strreplace (%simpleName, "\xf7", "+");
	%simpleName = strreplace (%simpleName, "\xc0", "A");
	%simpleName = strreplace (%simpleName, "\xc1", "A");
	%simpleName = strreplace (%simpleName, "\xc2", "A");
	%simpleName = strreplace (%simpleName, "\xc3", "A");
	%simpleName = strreplace (%simpleName, "\xc4", "A");
	%simpleName = strreplace (%simpleName, "\xc5", "A");
	%simpleName = strreplace (%simpleName, "\xc6", "AE");
	%simpleName = strreplace (%simpleName, "\xc7", "C");
	%simpleName = strreplace (%simpleName, "\xc9", "E");
	%simpleName = strreplace (%simpleName, "\xca", "E");
	%simpleName = strreplace (%simpleName, "\xcb", "E");
	%simpleName = strreplace (%simpleName, "\xcc", "I");
	%simpleName = strreplace (%simpleName, "\xcd", "I");
	%simpleName = strreplace (%simpleName, "\xce", "I");
	%simpleName = strreplace (%simpleName, "\xcf", "I");
	%simpleName = strreplace (%simpleName, "\xd0", "D");
	%simpleName = strreplace (%simpleName, "\xd1", "N");
	%simpleName = strreplace (%simpleName, "\xf1", "N");
	%simpleName = strreplace (%simpleName, "\xd2", "O");
	%simpleName = strreplace (%simpleName, "\xd3", "O");
	%simpleName = strreplace (%simpleName, "\xd4", "O");
	%simpleName = strreplace (%simpleName, "\xd5", "O");
	%simpleName = strreplace (%simpleName, "\xd6", "O");
	%simpleName = strreplace (%simpleName, "\xd8", "O");
	%simpleName = strreplace (%simpleName, "\xf2", "O");
	%simpleName = strreplace (%simpleName, "\xf3", "O");
	%simpleName = strreplace (%simpleName, "\xf4", "O");
	%simpleName = strreplace (%simpleName, "\xf5", "O");
	%simpleName = strreplace (%simpleName, "\xf6", "O");
	%simpleName = strreplace (%simpleName, "\xf8", "O");
	%simpleName = strreplace (%simpleName, "\xf0", "O");
	%simpleName = strreplace (%simpleName, "\xd9", "U");
	%simpleName = strreplace (%simpleName, "\xda", "U");
	%simpleName = strreplace (%simpleName, "\xdb", "U");
	%simpleName = strreplace (%simpleName, "\xdc", "U");
	%simpleName = strreplace (%simpleName, "\xe0", "A");
	%simpleName = strreplace (%simpleName, "\xe1", "A");
	%simpleName = strreplace (%simpleName, "\xe2", "A");
	%simpleName = strreplace (%simpleName, "\xe3", "A");
	%simpleName = strreplace (%simpleName, "\xe4", "A");
	%simpleName = strreplace (%simpleName, "\xe5", "A");
	%simpleName = strreplace (%simpleName, "\xe6", "AE");
	%simpleName = strreplace (%simpleName, "\xe7", "C");
	%simpleName = strreplace (%simpleName, "\xe8", "E");
	%simpleName = strreplace (%simpleName, "\xe9", "E");
	%simpleName = strreplace (%simpleName, "\xea", "E");
	%simpleName = strreplace (%simpleName, "\xeb", "E");
	%simpleName = strreplace (%simpleName, "\xec", "I");
	%simpleName = strreplace (%simpleName, "\xed", "I");
	%simpleName = strreplace (%simpleName, "\xee", "I");
	%simpleName = strreplace (%simpleName, "\xef", "I");
	%simpleName = strreplace (%simpleName, "\xf9", "U");
	%simpleName = strreplace (%simpleName, "\xfa", "U");
	%simpleName = strreplace (%simpleName, "\xfb", "U");
	%simpleName = strreplace (%simpleName, "\xfc", "U");
	%simpleName = strreplace (%simpleName, "\xfd", "Y");
	%simpleName = strreplace (%simpleName, "\xdd", "Y");
	%simpleName = strreplace (%simpleName, "\xfe", "P");
	%simpleName = strreplace (%simpleName, "\xdf", "B");
	%simpleName = strreplace (%simpleName, "\x8c", "CE");
	%simpleName = strreplace (%simpleName, "\x9c", "CE");

	// It's that simple!

	%client.simpleName = %simpleName;

	return %client.simpleName;
}
