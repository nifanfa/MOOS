char mytoupper(char c) {
	if (c >= 'a' && c <= 'z')
		return (c = c + 'A' - 'a');
	else
		return c;

}