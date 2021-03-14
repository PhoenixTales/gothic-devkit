# file.py: File operations.
# This file also provides GUI to setup the list of the texture directories.
#-------------------------------------------------------------------------------------------------------
# This file is a part of the KrxImpExp package.
# Author: Vitaly Baranov
# License: GPL
#-------------------------------------------------------------------------------------------------------


# Import required modules
import bpy
import os
import sys
from os import path, system
from struct import pack, unpack

# Does the file exist? Returns True or False.
def file_exists(filename):
	return path.exists(filename)

# Deletes the file if it exists.
def delete_file(filename):
	try:
		return remove(filename)
	except:
		return None
	
# Returns the size of the file, in bytes.
def get_file_size(filename):
	try:
		return path.getsize(filename)
	except:
		return 0

# Opens the file. Returns None if failed.
def open_file(filename, openmode):
	newopenmode = openmode.replace('t', 'b')
	try:
		return open(filename, newopenmode)
	except:
		return None

# Closes the file
def close_file(strm):
	strm.close()

# Returns the read or write position in the file. The file's beginning has position 0.
def file_tell(strm):
	return strm.tell()

# Sets the read or write position in the file. The file's beginning has position 0.
def file_seek(strm, ofs):
	strm.seek(ofs)

# Get current encoding
def get_encoding(strm):
	return sys.getfilesystemencoding()
	
# Writes signed 1-byte integer.
def write_signed_char(strm, i):
	strm.write(pack("<b", i))
	return True

# Writes unsigned 1-byte integer.
def write_unsigned_char(strm, i):
	strm.write(pack("<B", i))
	return True

# Writes signed 2-byte integer.
def write_signed_short(strm, i):
	strm.write(pack("<h", i))
	return True

# Writes unsigned 2-byte integer.
def write_unsigned_short(strm, i):
	strm.write(pack("<H", i))
	return True

# Writes signed 4-byte integer.
def write_signed_long(strm, i):
	strm.write(pack("<i", i))
	return True

# Writes unsigned 4-byte integer.
def write_unsigned_long(strm, i):
	strm.write(pack("<I", i))
	return True

# Writes 4-byte floating point number.
def write_float(strm, f):
	strm.write(pack("<f", f))
	return True

# Writes ANSI string with terminating zero character.
def write_stringz(strm, s):
	buf = s.encode(get_encoding(strm), errors="replace")
	buf = b"".join([buf, b"\x00"])
	strm.write(buf)
	return True
	
# Writes ANSI string with terminating CR character. 
def write_line(strm, s):
	buf = (s + os.linesep).encode(get_encoding(strm), errors="replace")
	strm.write(buf)
	return True

# Reads signed 1-byte integer.
def read_signed_char(strm):
	i = strm.read(1)
	if(len(i) != 1):
		return None
	return unpack("<b", i)[0]

# Reads unsigned 1-byte integer.
def read_unsigned_char(strm):
	i = strm.read(1)
	if(len(i) != 1):
		return None
	return unpack("<B", i)[0]

# Reads signed 2-byte integer.
def read_signed_short(strm):
	i = strm.read(2)
	if(len(i) != 2):
		return None
	return unpack("<h", i)[0]

# Reads unsigned 2-byte integer.
def read_unsigned_short(strm):
	i = strm.read(2)
	if(len(i) != 2):
		return None
	return unpack("<H", i)[0]
	
# Reads signed 4-byte integer.
def read_signed_long(strm):
	i = strm.read(4)
	if(len(i) != 4):
		return None
	return unpack("<i", i)[0]

# Reads unsigned 4-byte integer.
def read_unsigned_long(strm):
	i = strm.read(4)
	if(len(i) != 4):
		return None
	return unpack("<I", i)[0]

# Reads 4-byte floating point number.
def read_float(strm):
	f = strm.read(4)
	if(len(f) != 4):
		return None
	return unpack("<f", f)[0]

# Reads ANSI string terminated zero character.
def read_stringz(strm):
	pos = strm.tell()
	buf = strm.read(1024)
	size_read = len(buf)
	slen = buf.find(b"\x00")
	if slen == -1:
		return None
	strm.seek(pos + slen + 1)
	buf = buf[:slen]
	return buf.decode(get_encoding(strm), errors = "replace")

# Reads ANSI string terminated CR character.
def read_line(strm):
	pos = strm.tell()
	buf = strm.read(1024)
	size_read = len(buf)
	slen = buf.find(b"\n")
	if slen == -1:
		slen = buf.find(b"\r")
	if slen != -1:
		strm.seek(pos + slen + 1)
		buf = buf[:slen]
	s = buf.decode(get_encoding(strm), errors = "replace")
	if s[-1] == '\r' or s[-1] == '\n':
		s = s[:-1]
	return s

	