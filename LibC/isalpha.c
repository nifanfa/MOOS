/*-
 * See the file LICENSE for redistribution information.
 *
 * Copyright (c) 2005,2007 Oracle.  All rights reserved.
 *
 * $Id: isalpha.c,v 1.4 2007/05/17 15:14:54 bostic Exp $
 */

 /*
  * isalpha --
  *
  * PUBLIC: #ifndef HAVE_ISALPHA
  * PUBLIC: int isalpha __P((int));
  * PUBLIC: #endif
  */
int
myisalpha(c)
int c;
{
	/*
	 * Depends on ASCII-like character ordering.
	 */
	return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ? 1 : 0);
}