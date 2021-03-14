
from KrxImpExp.impexp import *

def FormatMsg(fmt, args):
	msg = (fmt)
	for i in range((0), (9)):
		argtempl = ("%" + (int_to_string(i + 1)))
		argpos = ((msg).find(argtempl))
		if(argpos != -1):
			msg = ((msg)[0:argpos] + ((args)[i]) + (msg)[argpos + 2:len(msg)])

	return (msg)

def FormatMsg0(fmt):
	args = ([])
	return (FormatMsg(fmt, args))

def FormatMsg1(fmt, arg1):
	args = ([])
	args.append(arg1)
	return (FormatMsg(fmt, args))

def FormatMsg2(fmt, arg1, arg2):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	return (FormatMsg(fmt, args))

def FormatMsg3(fmt, arg1, arg2, arg3):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	return (FormatMsg(fmt, args))

def FormatMsg4(fmt, arg1, arg2, arg3, arg4):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	return (FormatMsg(fmt, args))

def FormatMsg5(fmt, arg1, arg2, arg3, arg4, arg5):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	args.append(arg5)
	return (FormatMsg(fmt, args))

def FormatMsg6(fmt, arg1, arg2, arg3, arg4, arg5, arg6):
	args = ([])
	args.append(arg1)
	args.append(arg2)
	args.append(arg3)
	args.append(arg4)
	args.append(arg5)
	args.append(arg6)
	return (FormatMsg(fmt, args))

class TFile:
	def __MoveFilePos(self, ofs):
		self. __pos = (self. __pos + ofs)
		if(self. __size < self. __pos):
			self. __size = (self. __pos)

	def Init(self):
		self. __stream = (None)
		self. __name = ("")
		self. __mode = ("")
		self. __size = (0)
		self. __pos = (0)

	def Open(self, filename, mode):
		if(self. __stream != None):
			self. Close()
		
		filesz = (get_file_size(filename))
		self. __stream = (open_file(filename, mode))
		if(self. __stream == None):
			cr = ((mode).find("w") != -1)
			if(cr):
				raise RuntimeError(FormatMsg1("Could not open file for write.\nFile path: \"%1\".", filename))
			else:
				raise RuntimeError(FormatMsg1("Could not open file for read.\nFile path: \"%1\".", filename))

		self. __name = (filename)
		self. __mode = (mode)
		self. __size = (filesz)
		self. __pos = (0)

	def IsOpened(self):
		return (self. __stream != None)

	def Close(self):
		if(self. __stream != None):
			close_file(self. __stream)
			self. Init()

	def GetName(self):
		return (self. __name)

	def GetMode(self):
		return (self. __mode)

	def GetSize(self):
		return (self. __size)

	def GetPos(self):
		return (self. __pos)

	def SetPos(self, pos):
		if(pos != self. __pos):
			if(pos < 0 or pos > self. __size):
				raise RuntimeError(FormatMsg4("Attempt to seek file pointer out of file.\nFile path: \"%1\".\nPosition of file pointer: %2.\nAllowable range: %3..%4.", self. __name, ("0x"+hex(pos)[2: ].upper()), ("0x"+hex(0)[2: ].upper()), ("0x"+hex(self. __size)[2: ].upper())))
			
			file_seek(self. __stream, pos);
			self. __pos = (pos)

	def Eof(self):
		return (self. __pos == self. __size)

	def WriteSignedChar(self, i):
		if(i < -128 or i > 127):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "-128", "127"))
		
		b = ((write_signed_char(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "1"))
		
		self. __MoveFilePos(1)

	def WriteUnsignedChar(self, i):
		if(i < 0 or i > 255):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "0", "255"))
		
		b = ((write_unsigned_char(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "1"))
		
		self. __MoveFilePos(1)

	def WriteBool(self, b):
		i = (0)
		if(b):
			i = (1)
		
		self. WriteUnsignedChar(i)

	def WriteSignedShort(self, i):
		if(i < -32768 or i > 32767):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "-32768", "32767"))
		
		b = ((write_signed_short(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "2"))
		
		self. __MoveFilePos(2)

	def WriteUnsignedShort(self, i):
		if(i < 0 or i > 65535):
			raise RuntimeError(FormatMsg4("Could not write an integer to file.\nAn integer is out of range.\nFile path: \"%1\".\nInteger: %2.\nAllowable range: %3..%4.", self. __name, (int_to_string(i)), "0", "65535"))
		
		b = ((write_unsigned_short(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "2"))
		
		self. __MoveFilePos(2)

	def WriteSignedLong(self, i):
		b = ((write_signed_long(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteUnsignedLong(self, i):
		b = ((write_unsigned_long(self. __stream, i)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteFloat(self, f):
		b = ((write_float(self. __stream, f)))
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, "4"))
		
		self. __MoveFilePos(4)

	def WriteString(self, str):
		b = ((write_stringz(self. __stream, str)))
		sz = (len(str) + 1)
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, (int_to_string(sz))))
		
		self. __MoveFilePos(sz)

	def WriteLine(self, str):
		b = ((write_line(self. __stream, str)))
		sz = (len(str) + 2)
		if( not (b)):
			raise RuntimeError(FormatMsg2("Could not write data to file.\nFile path: \"%1\".\nSize of data: %2.", self. __name, (int_to_string(sz))))
		
		self. __MoveFilePos(sz)

	def ReadSignedChar(self):
		i = ((read_signed_char(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "1"))
		
		self. __MoveFilePos(1)
		return (i)

	def ReadUnsignedChar(self):
		i = ((read_unsigned_char(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "1"))
		
		self. __MoveFilePos(1)
		return (i)

	def ReadBool(self):
		i = (self. ReadUnsignedChar())
		return (i != 0)

	def ReadSignedShort(self):
		i = ((read_signed_short(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "2"))
		
		self. __MoveFilePos(2)
		return (i)

	def ReadUnsignedShort(self):
		i = ((read_unsigned_short(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "2"))
		
		self. __MoveFilePos(2)
		return (i)

	def ReadSignedLong(self):
		i = ((read_signed_long(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (i)

	def ReadUnsignedLong(self):
		i = ((read_unsigned_long(self. __stream)))
		if(i == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (i)

	def ReadFloat(self):
		f = ((read_float(self. __stream)))
		if(f == None):
			raise RuntimeError(FormatMsg3("Could not read data from file.\nFile path: \"%1\".\nPosition in file: %2.\nSize of data: %3.", self. __name, ("0x"+hex(self. __pos)[2: ].upper()), "4"))
		
		self. __MoveFilePos(4)
		return (f)

	def ReadString(self):
		str = ((read_stringz(self. __stream)))
		if(str == None):
			raise RuntimeError(FormatMsg2("Could not read a null-terminated string from file.\nThe string seems to be too long.\nFile path: \"%1\".\nPosition in file: %2.", self. __name, ("0x"+hex(self. __pos)[2: ].upper())))
		
		sz = (len(str) + 1)
		self. __MoveFilePos(sz)
		return (str)

	def ReadLine(self):
		str = ((read_line(self. __stream)))
		if(str == None):
			raise RuntimeError(FormatMsg2("Could not read a CR+LF ended line from file.\nThe line seems to be too long.\nFile path: \"%1\".\nPosition in file: %2.", self. __name, ("0x"+hex(self. __pos)[2: ].upper())))
		
		self. __pos = (file_tell(self. __stream))
		return (str)

def NewFile():
	fl = (TFile())
	fl.Init()
	return (fl)

class TObjectStats:
	def Init(self):
		self. __nameInFile = ("")
		self. __nameInScene = ("")
		self. __numMtls = (0)
		self. __numFaces = (0)
		self. __numVertsInFile = (0)
		self. __numVertsInScene = (0)

	def Write(self, f):
		f.WriteString(self. __nameInFile)
		f.WriteString(self. __nameInScene)
		f.WriteUnsignedLong(self. __numMtls)
		f.WriteUnsignedLong(self. __numFaces)
		f.WriteUnsignedLong(self. __numVertsInFile)
		f.WriteUnsignedLong(self. __numVertsInScene)

	def Read(self, f):
		self. __nameInFile = (f.ReadString())
		self. __nameInScene = (f.ReadString())
		self. __numMtls = (f.ReadUnsignedLong())
		self. __numFaces = (f.ReadUnsignedLong())
		self. __numVertsInFile = (f.ReadUnsignedLong())
		self. __numVertsInScene = (f.ReadUnsignedLong())

	def SetNameInFile(self, nameInFile):
		self. __nameInFile = (nameInFile)

	def GetNameInFile(self):
		return (self. __nameInFile)

	def SetNameInScene(self, nameInScene):
		self. __nameInScene = (nameInScene)

	def GetNameInScene(self):
		return (self. __nameInScene)

	def SetNumMtls(self, numMtls):
		self. __numMtls = (numMtls)

	def GetNumMtls(self):
		return (self. __numMtls)

	def SetNumFaces(self, numFaces):
		self. __numFaces = (numFaces)

	def GetNumFaces(self):
		return (self. __numFaces)

	def SetNumVertsInFile(self, numVertsInFile):
		self. __numVertsInFile = (numVertsInFile)

	def GetNumVertsInFile(self):
		return (self. __numVertsInFile)

	def SetNumVertsInScene(self, numVertsInScene):
		self. __numVertsInScene = (numVertsInScene)

	def GetNumVertsInScene(self):
		return (self. __numVertsInScene)

def NewObjectStats():
	objectStats = (TObjectStats())
	objectStats.Init()
	return (objectStats)

class TZENChunk:
	def Init(self):
		self. __chunkPos = (0)
		self. __chunkSize = (0)
		self. __chunkName = ("")
		self. __className = ("")
		self. __classVersion = (0)
		self. __objIndex = (0)

	def GetPos(self):
		return (self. __chunkPos)

	def SetPos(self, chunkPos):
		self. __chunkPos = (chunkPos)

	def GetSize(self):
		return (self. __chunkSize)

	def SetSize(self, chunkSize):
		self. __chunkSize = (chunkSize)

	def GetChunkName(self):
		return (self. __chunkName)

	def SetChunkName(self, chunkName):
		self. __chunkName = (chunkName)

	def GetClassName(self):
		return (self. __className)

	def SetClassName(self, className):
		self. __className = (className)

	def GetClassVersion(self):
		return (self. __classVersion)

	def SetClassVersion(self, classVersion):
		self. __classVersion = (classVersion)

	def GetObjectIndex(self):
		return (self. __objIndex)

	def SetObjectIndex(self, objIndex):
		self. __objIndex = (objIndex)

def NewZENChunk():
	zchunk = (TZENChunk())
	zchunk.Init()
	return (zchunk)

class TZENArchive:
	def __ReadLine(self, file):
		ln = ("")
		ch = ("")
		while( not (file.Eof())):
			ch = (file.ReadUnsignedChar())
			if(ch != 0x0A):
				ln += ((chr(ch)));
			else:
				break

		return (ln)

	def __ReadCommonHeader(self, file):
		line = (self. __ReadLine(file))
		if(line != "ZenGin Archive"):
			raise RuntimeError(FormatMsg1("File is not a ZenGin archive.\nFile name: \"%1\".", file.GetName()))

		pos = (file.GetPos())
		line = (self. __ReadLine(file))
		if((line)[0:3] != "ver"):
			raise RuntimeError(FormatMsg2("Cannot read archive, 'ver' keyword expected.\nPosition: %1.\nFile name: \"%2\".", ("0x"+hex(pos)[2: ].upper()), file.GetName()))
		
		self. __zenVersion = ((string_to_int((line)[3:len(line)])))
		
		line = (self. __ReadLine(file))
		
		pos = (file.GetPos())
		line = (self. __ReadLine(file))
		if(line == "BINARY"):
			self. __zenMode = (0)
		elif(line == "ASCII"):
			self. __zenMode = (1)
		elif(line == "BIN_SAFE"):
			self. __zenMode = (3)
		else:
			raise RuntimeError(FormatMsg2("Unknown archive mode.\nPosition: %1.\nFile name: \"%2\".", ("0x"+hex(pos)[2: ].upper()), file.GetName()))

		self. __saveGame = (False)
		while( not (file.Eof())):
			pos = (file.GetPos())
			line = (self. __ReadLine(file))
			if(line == "END"):
				break
			elif((line)[0:7] == "objects" and self. __zenVersion == 0):
				file.SetPos(pos)
				break
			elif((line)[0:8] == "saveGame"):
				if((string_to_int((line)[8:len(line)])) == 1):
					self. __saveGame = (True)

	def __ReadHeader_ASCII_Binary(self, file):
		pos = (file.GetPos())
		line = (self. __ReadLine(file))
		self. __numObjects = (0)
		if((line)[0:7] == "objects"):
			strObjects = ((line)[7:len(line)])
			posSpace = ((strObjects).find(" "))
			if(posSpace != -1):
				strObjects = ((strObjects)[0:posSpace])
			
			self. __numObjects = ((string_to_int(strObjects)))
		else:
			raise RuntimeError(FormatMsg2("Cannot read archive, 'objects' keyword expected.\nPosition: %1.\nFile name: \"%2\".", ("0x"+hex(pos)[2: ].upper()), file.GetName()))
		
		line = (self. __ReadLine(file)) 
		line = (self. __ReadLine(file)) 

	def __ReadHeader_BinSafe(self, file):
		binSafeVersion = (file.ReadUnsignedLong())
		self. __numObjects = (file.ReadUnsignedLong())
		mapPos = (file.ReadUnsignedLong())

	def __ReadString_ASCII(self, file):
		str = (self. __ReadLine(file))
		while((str)[0:1] == "\t"):
			str = ((str)[1:len(str)])
		
		return (str)

	def __ReadString_Binary(self, file):
		str = (file.ReadString())
		return (str)

	def __ReadString_BinSafe(self, file):
		pos = (file.GetPos())
		str = ("")
		typ = (file.ReadUnsignedChar())
		if(typ != 0x01):
			raise RuntimeError(FormatMsg2("A string expected.\nPosition: %1.\nFile name: \"%2\".", ("0x"+hex(pos)[2: ].upper()), file.GetName()))
		
		len = (file.ReadUnsignedShort())
		for i in range((0), (len)):
			str += ((chr(file.ReadUnsignedChar())));
		
		return (str)

	def __ReadString(self, file):
		if(self. __zenMode == 1):
			return (self. __ReadString_ASCII(file))
		elif(self. __zenMode == 0):
			return (self. __ReadString_Binary(file))
		else:
			return (self. __ReadString_BinSafe(file))

	def __ReadChunkStart_ASCII_BinSafe(self, file):
		emptyChunk = (NewZENChunk())
		chunk = (NewZENChunk())
		chunk.SetPos(file.GetPos())
		str = (self. __ReadString(file))
		
		if(str == "[]"):
			return (emptyChunk)

		if((str)[0:1] != "[" or (str)[len(str) - 1:len(str)] != "]"):
			return (emptyChunk)
		
		str = ((str)[1:len(str) - 1])
		
		posSpace = ((str).find(" "))
		if(posSpace == -1 or posSpace == 0):
			return (emptyChunk)
		
		chunk.SetChunkName((str)[0:posSpace])
		str = ((str)[posSpace + 1:len(str)])
		
		posSpace = ((str).find(" "))
		if(posSpace == -1 or posSpace == 0):
			return (emptyChunk)
		
		chunk.SetClassName((str)[0:posSpace])
		str = ((str)[posSpace + 1:len(str)])
		
		posSpace = ((str).find(" "))
		if(posSpace == -1 or posSpace == 0):
			return (emptyChunk)
		
		chunk.SetClassVersion((string_to_int((str)[0:posSpace])))
		str = ((str)[posSpace + 1:len(str)])
		
		chunk.SetObjectIndex((string_to_int((str)[0:posSpace])))
		return (chunk)

	def __ReadChunkStart_Binary(self, file):
		chunk = (NewZENChunk())
		chunk.SetPos(file.GetPos())
		chunk.SetSize(file.ReadUnsignedLong())
		chunk.SetClassVersion(file.ReadUnsignedShort())
		chunk.SetObjectIndex(file.ReadUnsignedLong())
		chunk.SetChunkName(file.ReadString())
		chunk.SetClassName(file.ReadString())
		return (chunk)

	def __ReadChunkEnd_Binary(self, file, chunk):
		file.SetPos(chunk.GetPos() + chunk.GetSize())

	def Init(self):
		self. __zenVersion = (0)
		self. __zenMode = (0)
		self. __saveGame = (False)
		self. __numObjects = (0)

	def ReadHeader(self, file):
		self. __ReadCommonHeader(file)
		if(self. __zenMode == 3):
			self. __ReadHeader_BinSafe(file)
		else:
			self. __ReadHeader_ASCII_Binary(file)

	def ReadString(self, file):
		return (self. __ReadString(file))

	def ReadChunkStart(self, file):
		if(self. __zenMode == 0):
			return (self. __ReadChunkStart_Binary(file))
		else:
			return (self. __ReadChunkStart_ASCII_BinSafe(file))

	def ReadChunkEnd(self, file, chunk):
		if(self. __zenMode == 0):
			self. __ReadChunkEnd_Binary(file, chunk)

def NewZENArchive():
	zarc = (TZENArchive())
	zarc.Init()
	return (zarc)

class TSpaceTransform:
	def Init(self):
		self. __setupUnit = (5)
		self. __systemUnitsPerFileUnit = (1)
		self. __fileUnitsPerSystemUnit = (1)

	def Write(self, f):
		f.WriteSignedLong(self. __setupUnit)
		f.WriteFloat(self. __systemUnitsPerFileUnit)
		f.WriteFloat(self. __fileUnitsPerSystemUnit)

	def Read(self, f):
		self. __setupUnit = (f.ReadSignedLong())
		self. __systemUnitsPerFileUnit = (f.ReadFloat())
		self. __fileUnitsPerSystemUnit = (f.ReadFloat())

	def GetSetupUnit(self):
		return (self. __setupUnit)

	def SetSetupUnit(self, setupUnit):
		self. __setupUnit = (setupUnit)

	def GetSystemUnitsPerFileUnit(self):
		return (self. __systemUnitsPerFileUnit)

	def SetSystemUnitsPerFileUnit(self, scaleCoef):
		self. __systemUnitsPerFileUnit = (scaleCoef)
		self. __fileUnitsPerSystemUnit = ((int_to_float(1)) / scaleCoef)

	def GetFileUnitsPerSystemUnit(self):
		return (self. __fileUnitsPerSystemUnit)

	def SetFileUnitsPerSystemUnit(self, scaleCoef):
		self. __fileUnitsPerSystemUnit = (scaleCoef)
		self. __systemUnitsPerFileUnit = ((int_to_float(1)) / scaleCoef)

def NewSpaceTransform():
	st = (TSpaceTransform())
	st.Init()
	return (st)

def DelObjects(objects):
	i = ((len(objects)) - 1)
	while(i >= 0):
		obj = (((objects)[i]))
		delete_object(obj)
		i = (i - 1)

class TMSHFileLoader:
	def __SetSpaceTransform(self, spaceTransform):
		setupUnit = (spaceTransform.GetSetupUnit())
		if(setupUnit == 1):
			pass
		elif(setupUnit == 2):
			pass
		elif(setupUnit == 3):
			pass
		elif(setupUnit == 4):
			pass
		elif(setupUnit == 5):
			pass
		elif(setupUnit == 6):
			pass
		elif(setupUnit == 7):
			pass
		
		self. __scaleCoef = (spaceTransform.GetSystemUnitsPerFileUnit())

	def __ReadMshVersion(self):
		self. __mshVersion = (self. __file.ReadUnsignedShort())
		if((self. __mshVersion != 0x0009) and (self. __mshVersion != 0x0109)):
			raise RuntimeError(FormatMsg2("Msh version is not supported.\nMsh version: %1.\nFile name: \"%2\".", ("0x"+hex(self. __mshVersion)[2: ].upper()), self. __file.GetName()))

	def __ReadMaterials(self):
		zarc = (NewZENArchive())
		zarc.ReadHeader(self. __file)
		numMaterials = (self. __file.ReadUnsignedLong())
		for i in range((0), (numMaterials)):
			caption = ("Reading materials")
			show_progress_bar(caption, 100 * i / numMaterials)
			name = (zarc.ReadString(self. __file))
			pos = (self. __file.GetPos())
			zchunk = (zarc.ReadChunkStart(self. __file))
			if(zchunk.GetClassName() != "zCMaterial"):
				raise RuntimeError(FormatMsg2("A chunk of class \"zCMaterial\" expected here.\nPosition: %1.\nFile name: \"%2\".", ("0x"+hex(pos)[2: ].upper()), self. __file.GetName()))
			
			name = (self. __file.ReadString())
			matGroup = (self. __file.ReadUnsignedChar())
			blue = (self. __file.ReadUnsignedChar())
			green = (self. __file.ReadUnsignedChar())
			red = (self. __file.ReadUnsignedChar())
			alpha = (self. __file.ReadUnsignedChar())
			clr = (new_color(red / 255., green / 255., blue / 255.))
			smoothAngle = (self. __file.ReadFloat())
			texture = (self. __file.ReadString())
			zarc.ReadChunkEnd(self. __file, zchunk)
			
			mtl = (new_material(name))
			self. __impMtls.append(mtl)
			set_diffuse_color(mtl, clr)
			set_diffuse_map_filename(mtl, texture)

	def __CreateObject(self):
		self. __curNameInFile = ("zengin")
		if((find_object_by_name(self. __curNameInFile)) == None):
			self. __curNameInScene = (self. __curNameInFile)
		else:
			self. __curNameInScene = ((unique_object_name(self. __curNameInFile)))
		
		self. __curObj = (new_mesh_object(self. __curNameInScene))
		self. __curMesh = (MeshData(self. __curObj))
		self. __impObjects.append(self. __curObj)

	def __ReadVertices(self):
		caption = (FormatMsg1("Reading '%1'", self. __curNameInFile))
		numVerts = (self. __file.ReadUnsignedLong())
		self. __curMesh.set_num_verts(numVerts)
		for i in range((0), (numVerts)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 25 * i / numVerts)
			
			x = (self. __file.ReadFloat() * self. __scaleCoef)
			z = (self. __file.ReadFloat() * self. __scaleCoef)
			y = (self. __file.ReadFloat() * self. __scaleCoef)
			pt = (new_vector(x, y, z))
			(self. __curMesh.set_vert(i, pt))

	def __ReadUVMapping(self):
		numTVerts = (self. __file.ReadUnsignedLong())
		self. __curMesh.set_num_tverts(numTVerts)
		caption = (FormatMsg1("Reading '%1'", self. __curNameInFile))
		for i in range((0), (numTVerts)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 25 + 25 * i / numTVerts)
			
			u = (self. __file.ReadFloat())
			v = ( -self. __file.ReadFloat())
			clrStaticLight = (self. __file.ReadUnsignedLong())
			normalX = (self. __file.ReadFloat())
			normalY = (self. __file.ReadFloat())
			normalZ = (self. __file.ReadFloat())
			uvvert = (new_uvvert(u, v))
			self. __curMesh.set_tvert(i, uvvert) 

	def __CountTriFaces(self):
		caption = (FormatMsg1("Reading '%1'", self. __curNameInFile))
		pos = (self. __file.GetPos())
		self. __numTriFaces = (0)
		numFaces = (self. __file.ReadUnsignedLong())
		for i in range((0), (numFaces)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 50 + 25 * i / numFaces)
			
			matID = (self. __file.ReadUnsignedShort())
			lightMapID = (self. __file.ReadUnsignedShort())
			bbox1 = (self. __file.ReadFloat())
			bbox2 = (self. __file.ReadFloat())
			bbox3 = (self. __file.ReadFloat())
			bbox4 = (self. __file.ReadFloat())
			flags = (0)
			if(self. __mshVersion == 0x0009):
				flags = (self. __file.ReadUnsignedShort())
			elif(self. __mshVersion == 0x0109):
				flags = (self. __file.ReadUnsignedChar())
			
			unused = (self. __file.ReadUnsignedShort())
			numVertsInFace = (self. __file.ReadUnsignedChar())
			self. __numTriFaces = (self. __numTriFaces + numVertsInFace - 2)
			
			if(self. __mshVersion == 0x0009):
				self. __file.SetPos(self. __file.GetPos() + 6 * numVertsInFace)
			elif(self. __mshVersion == 0x0109):
				self. __file.SetPos(self. __file.GetPos() + 8 * numVertsInFace)

		self. __file.SetPos(pos)

	def __ReadFaces(self):
		self. __curMesh.set_num_faces(self. __numTriFaces)
		self. __curMesh.set_num_tvfaces(self. __curMesh.get_num_faces())
		triFaceIndex = (0)
		
		numVerts = ((self. __curMesh.get_num_verts()))
		numTVerts = ((self. __curMesh.get_num_tverts()))
		numMaterials = ((len(self. __impMtls)))
		
		caption = (FormatMsg1("Reading '%1'", self. __curNameInFile))
		numFaces = (self. __file.ReadUnsignedLong())
		for i in range((0), (numFaces)):
			if(((i) & (31)) == 0):
				show_progress_bar(caption, 75 + 25 * i / numFaces)

			matID = (self. __file.ReadUnsignedShort())
			if(matID >= numMaterials):
				raise RuntimeError(FormatMsg4("Material ID is out of range while reading chunk 0xB050.\nMaterial ID: %1 (Allowable range: %2..%3).\nFile name: \"%4\".", (int_to_string(matID)), "0", (int_to_string(numMaterials - 1)), self. __file.GetName()))
			
			mat = (((self. __impMtls)[matID]))
			
			lightMapID = (self. __file.ReadUnsignedShort())
			bbox1 = (self. __file.ReadFloat())
			bbox2 = (self. __file.ReadFloat())
			bbox3 = (self. __file.ReadFloat())
			bbox4 = (self. __file.ReadFloat())
			if(self. __mshVersion == 0x0009):
				flags = (self. __file.ReadUnsignedShort())
			elif(self. __mshVersion == 0x0109):
				flags = (self. __file.ReadUnsignedChar())
			
			unused = (self. __file.ReadUnsignedShort())
			numVertsInFace = (self. __file.ReadUnsignedChar())
			vertIdx_0 = (0)
			tVertIdx_0 = (0)
			vertIdx_Prev = (0)
			tVertIdx_Prev = (0)
			
			for j in range((0), (numVertsInFace)):
				vertIdx = (0)
				if(self. __mshVersion == 0x0009):
					vertIdx = (self. __file.ReadUnsignedShort())
				elif(self. __mshVersion == 0x0109):
					vertIdx = (self. __file.ReadUnsignedLong())
				
				tVertIdx = (self. __file.ReadUnsignedLong())
				if(vertIdx >= numVerts):
					raise RuntimeError(FormatMsg4("Vertex index is out of range while reading chunk 0xB050.\nVertex index: %1 (Allowable range: %2..%3).\nFile name: \"%4\".", (int_to_string(vertIdx)), "0", (int_to_string(numVerts - 1)), self. __file.GetName()))
				
				if(tVertIdx >= numTVerts):
					raise RuntimeError(FormatMsg4("Texture vertex index is out of range while reading chunk 0xB050.\nTexture vertex index: %1 (Allowable range: %2..%3).\nFile name: \"%4\".", (int_to_string(tVertIdx)), "0", (int_to_string(numTVerts - 1)), self. __file.GetName()))
				
				if(j == 0):
					vertIdx_0 = (vertIdx)
					tVertIdx_0 = (tVertIdx)
				
				if(j >= 2):
					f = (new_face(vertIdx_0, vertIdx_Prev, vertIdx))
					self. __curMesh.set_face(triFaceIndex, f)
					tf = (new_tvface(tVertIdx_0, tVertIdx_Prev, tVertIdx))
					self. __curMesh.set_tvface(triFaceIndex, tf)
					self. __curMesh.set_face_material(triFaceIndex, mat)
					for k in range((0), (3)):
						pass
					
					triFaceIndex = (triFaceIndex + 1)
				
				vertIdx_Prev = (vertIdx)
				tVertIdx_Prev = (tVertIdx)

	def __EndReadingMesh(self):
		objects = ((self. __curMesh.update()))
		for i in range((0), ((len(objects)))):
			obj = (((objects)[i]))
			msh = (MeshData(obj))
			objStats = (NewObjectStats())
			objStats.SetNameInFile(self. __curNameInFile)
			objStats.SetNameInScene((get_object_name(obj)))
			objStats.SetNumMtls((calculate_num_materials(obj)))
			objStats.SetNumFaces((calculate_num_faces(obj)))
			objStats.SetNumVertsInFile((calculate_num_verts(obj)))
			objStats.SetNumVertsInScene(objStats.GetNumVertsInFile())
			self. __impObjectStats.append(objStats)
			if(i != 0):
				self. __impObjects.append(obj)

	def Init(self):
		self. __scaleCoef = (1.0)
		self. __file = (NewFile())
		self. __mshVersion = (0)
		self. __impMtls = ([])
		self. __impObjects = ([])
		self. __curObj = (None)
		self. __curNameInFile = ("")
		self. __curMesh = (None)
		self. __impObjectStats = ([])
		self. __numTriFaces = (0)

	def ReadMSHFile2(self, file, spaceTransform):
		self. Init()
		self. __SetSpaceTransform(spaceTransform)
		self. __file = (file)
		try:
			fileBeginning = (True)
			while( not (self. __file.Eof())):
				typ = (self. __file.ReadUnsignedShort())
				if(fileBeginning and typ != 0xB000):
					raise RuntimeError(FormatMsg1("File is not a compiled mesh.\nFile name: \"%1\".", self. __file.GetName()))
				
				fileBeginning = (False)
				sz = (self. __file.ReadUnsignedLong())
				chunkPos = (self. __file.GetPos())
				if(typ == 0xB000):
					self. __ReadMshVersion()
				elif(typ == 0xB020):
					self. __ReadMaterials()
				elif(typ == 0xB030):
					self. __CreateObject()
					self. __ReadVertices()
				elif(typ == 0xB040):
					self. __ReadUVMapping()
				elif(typ == 0xB050):
					self. __CountTriFaces()
					self. __ReadFaces()
				elif(typ == 0xB060):
					self. __EndReadingMesh()
					break
				
				self. __file.SetPos(chunkPos + sz)

		except RuntimeError as ex:
			raise

	def ReadMSHFile(self, filename, spaceTransform):
		self. Init()
		file = (NewFile())
		try:
			file.Open(filename, "rb")
			self. ReadMSHFile2(file, spaceTransform)
			file.Close()
		
		except RuntimeError as ex:
			file.Close()
			raise

	def GetObjectStats(self):
		return (self. __impObjectStats)

def NewMSHFileLoader():
	loader = (TMSHFileLoader())
	loader.Init()
	return (loader)

class TZENFileLoader:
	def Init(self):
		self. __file = (NewFile())
		self. __mshFileLoader = (NewMSHFileLoader())

	def ReadZENFile(self, filename, spaceTransform):
		self. Init()
		try:
			self. __file.Open(filename, "rb")
			zarc = (NewZENArchive())
			zarc.ReadHeader(self. __file)
			zchunk = (zarc.ReadChunkStart(self. __file))
			if(zchunk.GetClassName() != "oCWorld:zCWorld"):
				raise RuntimeError(FormatMsg1("A chunk of class \"oCWorld:zCWorld\" wasn't found.\nFile name: \"%1\".", self. __file.GetName()))
			
			zchunk2 = (zarc.ReadChunkStart(self. __file))
			if(zchunk2.GetChunkName() != "MeshAndBsp"):
				raise RuntimeError(FormatMsg1("Chunk \"MeshAndBsp\" wasn't found.\nFile name: \"%1\".", self. __file.GetName()))
			
			meshAndBspVer = (self. __file.ReadUnsignedLong())
			meshAndBspSize = (self. __file.ReadUnsignedLong())
			
			self. __mshFileLoader.ReadMSHFile2(self. __file, spaceTransform)
			self. __file.Close()
		
		except RuntimeError as ex:
			self. __file.Close()
			raise

	def GetObjectStats(self):
		return (self. __mshFileLoader.GetObjectStats())

def NewZENFileLoader():
	loader = (TZENFileLoader())
	loader.Init()
	return (loader)

class TMeshImporterDlgInput:
	def Init(self):
		self. __fileFormat = ("")
		self. __importFileName = ("")
		self. __importFileSize = (0)
		self. __curSceneMode = (1)
		self. __systemUnitsPerFileUnit = (1)

	def Write(self, f):
		f.WriteString(self. __fileFormat)
		f.WriteString(self. __importFileName)
		f.WriteUnsignedLong(self. __importFileSize)
		f.WriteUnsignedChar(self. __curSceneMode)
		f.WriteFloat(self. __systemUnitsPerFileUnit)

	def Read(self, f):
		self. __fileFormat = (f.ReadString())
		self. __importFileName = (f.ReadString())
		self. __importFileSize = (f.ReadUnsignedLong())
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __systemUnitsPerFileUnit = (f.ReadFloat())

	def SetFileFormat(self, fileFormat):
		self. __fileFormat = (fileFormat)

	def GetFileFormat(self):
		return (self. __fileFormat)

	def SetImportFileName(self, importFileName):
		self. __importFileName = (importFileName)

	def GetImportFileName(self):
		return (self. __importFileName)

	def SetImportFileSize(self, importFileSize):
		self. __importFileSize = (importFileSize)

	def GetImportFileSize(self):
		return (self. __importFileSize)

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetSystemUnitsPerFileUnit(self, scaleCoef):
		self. __systemUnitsPerFileUnit = (scaleCoef)

	def GetSystemUnitsPerFileUnit(self):
		return (self. __systemUnitsPerFileUnit)

def NewMeshImporterDlgInput():
	dlgInput = (TMeshImporterDlgInput())
	dlgInput.Init()
	return (dlgInput)

class TMeshImporterDlgOutput:
	def Init(self, dlgInput):
		self. __curSceneMode = (dlgInput.GetCurrentSceneMode())
		self. __continueImport = (True)
		self. __spaceTransform = (NewSpaceTransform())
		self. __spaceTransform.SetSystemUnitsPerFileUnit(dlgInput.GetSystemUnitsPerFileUnit())

	def Write(self, f):
		f.WriteUnsignedChar(self. __curSceneMode)
		self. __spaceTransform.Write(f)
		f.WriteBool(self. __continueImport)

	def Read(self, f):
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __spaceTransform.Read(f)
		self. __continueImport = (f.ReadBool())

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetSpaceTransform(self, spaceTransform):
		self. __spaceTransform = (spaceTransform)

	def GetSpaceTransform(self):
		return (self. __spaceTransform)

	def SetContinueImport(self, continueImport):
		self. __continueImport = (continueImport)

	def GetContinueImport(self):
		return (self. __continueImport)

def NewMeshImporterDlgOutput(dlgInput):
	dlgOutput = (TMeshImporterDlgOutput())
	dlgOutput.Init(dlgInput)
	return (dlgOutput)

class TMeshInfoDlgInput:
	def Init(self):
		self. __fileName = ("")
		self. __fileSize = (0)
		self. __exportMode = (False)
		self. __objectStats = ([])

	def Write(self, f):
		f.WriteString(self. __fileName)
		f.WriteUnsignedLong(self. __fileSize)
		f.WriteBool(self. __exportMode)
		f.WriteUnsignedLong((len(self. __objectStats)))
		for i in range((0), ((len(self. __objectStats)))):
			objst = (((self. __objectStats)[i]))
			objst.Write(f)

	def Read(self, f):
		self. __fileName = (f.ReadString())
		self. __fileSize = (f.ReadUnsignedLong())
		self. __exportMode = (f.ReadBool())
		self. __objectStats = ([])
		numObjects = (f.ReadUnsignedLong())
		for i in range((0), (numObjects)):
			objst = (NewObjectStats())
			objst.Read(f)
			self. __objectStats.append(objst)

	def SetFileName(self, fileName):
		self. __fileName = (fileName)

	def GetFileName(self):
		return (self. __fileName)

	def SetFileSize(self, fileSize):
		self. __fileSize = (fileSize)

	def GetFileSize(self):
		return (self. __fileSize)

	def SetExportMode(self, exportMode):
		self. __exportMode = (exportMode)

	def GetExportMode(self):
		return (self. __exportMode)

	def SetObjectStats(self, objectStats):
		self. __objectStats = (objectStats)

	def GetObjectStats(self):
		return (self. __objectStats)

def NewMeshInfoDlgInput():
	dlgInput = (TMeshInfoDlgInput())
	dlgInput.Init()
	return (dlgInput)

def RunUIExe():
	cmdline = ("")
	cmdline = (cmdline + "\"" + (get_root_dir() + "KrxImpExp\\wxImpExpUI.exe") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpDlgInput.bin") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpDlgOutput.bin") + "\"")
	cmdline = (cmdline + " ")
	cmdline = (cmdline + "\"" + (get_plugcfg_dir() + "KrxImpExpUI.cfg") + "\"")
	errCode = (system("\"" + cmdline + "\""))
	if(errCode != 0):
		raise RuntimeError(FormatMsg1("Could not execute:\n%1", (get_root_dir() + "KrxImpExp\\wxImpExpUI.exe")))

def register(): 
	register_importer("KrxZenImp", "ZEN", "Kerrax ZenGin World") 

def unregister(): 
	unregister_importer("KrxZenImp") 

if __name__ == "main": 
	register() 

def KrxZenImp(filename_param, quiet_param = False): 
	QUIET = quiet_param 
	IMPORT_FILE_NAME = filename_param 
	IMPORT_FILE_SIZE = get_file_size(IMPORT_FILE_NAME)
	
	dlgInput = (NewMeshImporterDlgInput())
	dlgInput.SetFileFormat("ZEN")
	dlgInput.SetImportFileName(IMPORT_FILE_NAME)
	dlgInput.SetImportFileSize(IMPORT_FILE_SIZE)
	dlgInput.SetSystemUnitsPerFileUnit(0.01)
	dlgInput.SetCurrentSceneMode(1)
	
	dlgOutput = (NewMeshImporterDlgOutput(dlgInput))
	inputFile = (NewFile())
	outputFile = (NewFile())
	if( not (QUIET)):
		try:
			show_progress_bar("Showing dialog", 0)
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("MeshImporterDlgInput")
			dlgInput.Write(inputFile)
			inputFile.Close()
			RunUIExe()
			outputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgOutput.bin"), "rb")
			structName = (outputFile.ReadString())
			dlgOutput.Read(outputFile)
			outputFile.Close()
			
			if( not (dlgOutput.GetContinueImport())):
				hide_progress_bar()
				return (2)

		except RuntimeError as ex:
			inputFile.Close()
			outputFile.Close()
			show_error_box("Kerrax ZEN Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
			hide_progress_bar()
			return (2)

	loader = (NewZENFileLoader())
	try:
		if(dlgOutput.GetCurrentSceneMode() == 1):
			reset_scene()

		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.ReadZENFile(IMPORT_FILE_NAME, spaceTransform)
	
	except RuntimeError as ex:
		show_error_box("Kerrax ZEN Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
		hide_progress_bar()
		return (0)

	if( not (QUIET)):
		show_progress_bar("Showing dialog", 98)
		dlgInfo = (NewMeshInfoDlgInput())
		dlgInfo.SetFileName(IMPORT_FILE_NAME)
		dlgInfo.SetFileSize(IMPORT_FILE_SIZE)
		dlgInfo.SetExportMode(False)
		dlgInfo.SetObjectStats(loader.GetObjectStats())
		try:
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("MeshInfoDlgInput")
			dlgInfo.Write(inputFile)
			inputFile.Close()
			RunUIExe()
		
		except RuntimeError as ex:
			inputFile.Close()
			show_error_box("Kerrax ZEN Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))

	hide_progress_bar()
	return (1)


