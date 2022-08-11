/*-
 * See the file LICENSE for redistribution information.
 *
 * Copyright (c) 2005,2007 Oracle.  All rights reserved.
 *
 * $Id: isdigit.c,v 1.4 2007/05/17 15:14:54 bostic Exp $
 */

 /*
  * isdigit --
  *
  * PUBLIC: #ifndef HAVE_ISDIGIT
  * PUBLIC: int isdigit __P((int));
  * PUBLIC: #endif
  */
int
myisdigit(c)
int c;
{
	/*
	 * Depends on ASCII-like character ordering.
	 */
	return (c >= '0' && c <= '9' ? 1 : 0);
}
