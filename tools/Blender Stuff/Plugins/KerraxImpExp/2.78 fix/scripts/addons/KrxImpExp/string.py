# string.py: string utilities.
#------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#------------------------------------------------------


# Returns an upper-case version of string.
def uppercase(s):
	return s.upper()

# Returns a lower-case version of string.
def lowercase(s):
	return s.lower()

# Compares strings with case sentive (returns 0 if the strings are equal, 
# -1 if the first string < the second string, 1 if the first string > the second string)
def strcmp(s1, s2):
	return (s1 > s2) - (s1 < s2)
	
# Compares strings with case insentive (returns 0 if the strings are equal, 
# -1 if the first string < the second string, 1 if the first string > the second string)
def stricmp(s1, s2):
	return strcmp(uppercase(s1), uppercase(s2))