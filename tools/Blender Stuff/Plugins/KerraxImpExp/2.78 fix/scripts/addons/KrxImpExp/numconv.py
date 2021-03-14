# numconv.py: simple utilities to convert numbers.
#--------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#--------------------------------------------------------------------------------------------------

def bool_to_int(b):
	return int(b)
	
def bool_to_float(b):
	return float(b)

def bool_to_string(b):
	if(bool(b)):
		return "true"
	else:
		return "false"

def int_to_bool(i):
	return (i != 0)
	
def int_to_float(i):
	return float(i)

def int_to_string(i):
	return str(int(i))

def float_to_bool(f):
	return (f != 0.0)

def float_to_int(f):
	return int(f + 0.5)

def float_to_string(f):
	return str(float(f))

def string_to_bool(s):
	if(s == "true" or s == "True" or s == "TRUE" or s == "yes" or s == "Yes" or s == "YES"):
		return True
	else:
		return False

def string_to_int(s):
	try:
		return int(s)
	except ValueError:
		return None
		
def string_to_float(s):
	try:
		return float(s)
	except ValueError:
		return None	