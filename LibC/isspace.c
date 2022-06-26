/*-
 * See the file LICENSE for redistribution information.
 *
 * Copyright (c) 2005,2007 Oracle.  All rights reserved.
 *
 * $Id: myisspace.c,v 1.4 2007/05/17 15:14:54 bostic Exp $
 */

 /*
  * myisspace --
  *
  * PUBLIC: #ifndef HAVE_ISSPACE
  * PUBLIC: int myisspace __P((int));
  * PUBLIC: #endif
  */
int
myisspace(c)
int c;
{
	return (c == '\t' || c == '\n' ||
		c == '\v' || c == '\f' || c == '\r' || c == ' ' ? 1 : 0);
}