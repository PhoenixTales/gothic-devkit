
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

class TMatRenParams:
	def __ExtendString(self, str, newLength):
		str2 = (str)
		while(len(str2) < newLength):
			str2 += (" ");
		
		return (str2)

	def __ParsePmlFile(self, pmlFilePath):
		f = (NewFile())
		try:
			f.Open(pmlFilePath, "rt")
			name = ("")
			texture = ("")
			while( not (f.Eof())):
				str = (f.ReadLine())
				if((str).find("[% zCMaterial") != -1):
					name = ("")
					texture = ("")
				elif((str).find("name=string:") != -1):
					pos = ((str).find(":"))
					name = ((str)[pos + 1:len(str)])
				elif((str).find("texture=string:") != -1):
					pos = ((str).find(":"))
					texture = ((str)[pos + 1:len(str)])
				elif((str).find("[]") != -1):
					if((name != "") and (texture != "")):
						alreadyInList = (False)
						for i in range((0), ((len(self. __names)))):
							if((((self. __names)[i]) == name) and (((self. __textures)[i]) == texture)):
								alreadyInList = (True)
								break

						if( not (alreadyInList)):
							self. __names.append(name)
							self. __textures.append(texture)

		except RuntimeError as ex:
			None
		
		f.Close()

	def __CompareMaterials(self, i, j):
		cmp = (stricmp(((self. __names)[i]), ((self. __names)[j])))
		if(cmp == 0):
			cmp = (stricmp(((self. __textures)[i]), ((self. __textures)[j])))
		
		return (cmp)

	def __SwapMaterials(self, i, j):
		tmp = (((self. __names)[i]))
		self. __names[i] = (((self. __names)[j]))
		self. __names[j] = (tmp)
		tmp = (((self. __textures)[i]))
		self. __textures[i] = (((self. __textures)[j]))
		self. __textures[j] = (tmp)

	def __SortMaterials(self):
		for i in range((0), ((len(self. __names)))):
			m = (i)
			for j in range((i + 1), ((len(self. __names)))):
				if(self. __CompareMaterials(m, j) > 0):
					m = (j)

			if(m != i):
				self. __SwapMaterials(m, i)

	def Init(self):
		self. __names = ([])
		self. __textures = ([])
		self. __autoNames = (False)

	def SaveTextFile(self, filename):
		f = (NewFile())
		try:
			f.Open(filename, "wt")
			str = (self. __ExtendString("Material  ", 41) + " | Texture")
			f.WriteLine(str)
			for i in range((0), ((len(self. __textures)))):
				str = (self. __ExtendString("\"" + ((self. __names)[i]) + "\"", 41))
				str = (str + " | \"" + ((self. __textures)[i]) + "\"")
				f.WriteLine(str)
			
			f.WriteLine("")
			f.WriteLine("AutoNames = " + (bool_to_string(self. __autoNames)))
		
		except RuntimeError as ex:
			None
		
		f.Close()

	def LoadTextFile(self, filename):
		self. Init()
		f = (NewFile())
		try:
			f.Open(filename, "rt")
			while( not (f.Eof())):
				str = (f.ReadLine())
				strU = (uppercase(str))
				if((strU).find("AUTONAMES") != -1):
					pos = ((str).find("="))
					if(pos != -1):
						strBoolVal = ((str)[pos + 1:len(str)])
						while((strBoolVal)[0:1] == " "):
							strBoolVal = ((strBoolVal)[1:len(strBoolVal)])
						
						while((strBoolVal)[len(strBoolVal) - 1:len(strBoolVal)] == " "):
							strBoolVal = ((strBoolVal)[0:len(strBoolVal) - 1])
						
						self. __autoNames = ((string_to_bool(strBoolVal)))
					
				elif((str).find("|") != -1):
					pos1 = ((str).find("\""))
					if(pos1 != -1):
						pos2 = (((str)[pos1 + 1:len(str)]).find("\"") + pos1 + 1)
						if(pos2 != -1):
							pos3 = (((str)[pos2 + 1:len(str)]).find("\"") + pos2 + 1)
							if(pos3 != -1):
								pos4 = (((str)[pos3 + 1:len(str)]).find("\"") + pos3 + 1)
								if(pos4 != -1):
									name = ((str)[pos1 + 1:pos2])
									texture = ((str)[pos3 + 1:pos4])
									self. __names.append(name)
									self. __textures.append(texture)

		except RuntimeError as ex:
			None
		
		f.Close()

	def LoadMaterialFilter(self, matLibIniPath):
		toolsDataDir = (matLibIniPath)
		pos = (len(toolsDataDir) - 1)
		while(pos >= 0):
			ch = ((toolsDataDir)[pos:pos + 1])
			if((ch == "\\") or (ch == "/")):
				toolsDataDir = ((toolsDataDir)[0:pos + 1])
				break
			
			pos = (pos - 1)

		fMatLibIni = (NewFile())
		try:
			fMatLibIni.Open(matLibIniPath, "rt")
			self. __names = ([])
			self. __textures = ([])
			while( not (fMatLibIni.Eof())):
				str = (fMatLibIni.ReadLine())
				pos = ((str).find("="))
				if(pos != -1):
					pmlFilePath = (toolsDataDir + (str)[0:pos] + ".pml")
					self. __ParsePmlFile(pmlFilePath)

			self. __SortMaterials()
		
		except RuntimeError as ex:
			None
		
		fMatLibIni.Close()

	def GetNumMaterials(self):
		return ((len(self. __names)))

	def SetNumMaterials(self, numMaterials):
		if((len(self. __names)) != numMaterials):
			self. __names = ([])
			self. __textures = ([])
			
			for i in range((0), (numMaterials)):
				self. __names.append("")
				self. __textures.append("")

	def GetName(self, index):
		return (((self. __names)[index]))

	def SetName(self, index, name):
		self. __names[index] = (name)

	def GetTexture(self, index):
		return (((self. __textures)[index]))

	def SetTexture(self, index, name):
		self. __textures[index] = (name)

	def GetAutoNames(self):
		return (self. __autoNames)

	def SetAutoNames(self, autoNames):
		self. __autoNames = (autoNames)

def NewMatRenParams():
	mr = (TMatRenParams())
	mr.Init()
	return (mr)

class T3DSWriteChunk:
	def Init(self):
		self. __chunkID = (0)
		self. __chunkPos = (0)
		self. __chunkSize = (0)

	def WriteBegin(self, file, chunkID):
		self. __chunkID = (chunkID)
		self. __chunkPos = (file.GetPos())
		self. __chunkSize = (0)
		file.WriteUnsignedShort(self. __chunkID)
		file.WriteUnsignedLong(self. __chunkSize)

	def WriteEnd(self, file):
		chunkEndPos = (file.GetPos())
		self. __chunkSize = (chunkEndPos - self. __chunkPos)
		file.SetPos(self. __chunkPos + 2)
		file.WriteUnsignedLong(self. __chunkSize)
		file.SetPos(chunkEndPos)

def New3DSWriteChunk():
	chunk = (T3DSWriteChunk())
	chunk.Init()
	return (chunk)

class TMaterialDesc:
	def Init(self):
		self. __materialName = ("")
		self. __diffuseMapFilename = ("")
		self. __diffuseColor = (black_color())

	def GetMaterialName(self):
		return (self. __materialName)

	def SetMaterialName(self, materialName):
		self. __materialName = (materialName)

	def GetDiffuseMapFilename(self):
		return (self. __diffuseMapFilename)

	def SetDiffuseMapFilename(self, filename):
		self. __diffuseMapFilename = (filename)

	def GetDiffuseColor(self):
		return (self. __diffuseColor)

	def SetDiffuseColor(self, clr):
		self. __diffuseColor = (clr)

def NewMaterialDesc():
	m = (TMaterialDesc())
	m.Init()
	return (m)

def GetDefaultMaterialName(obj):
	clr = ((get_wire_color(obj)))
	ri = ((float_to_int(get_red(clr) * 255)))
	gi = ((float_to_int(get_green(clr) * 255)))
	bi = ((float_to_int(get_blue(clr) * 255)))
	mtlName = ("R" + (int_to_string(ri)) + " G" + (int_to_string(gi)) + " B" + (int_to_string(bi)))
	return (mtlName)

def GetDefaultMaterial(obj):
	clr = ((get_wire_color(obj)))
	mtlName = (GetDefaultMaterialName(obj))
	mtlDesc = (NewMaterialDesc())
	mtlDesc.SetMaterialName(mtlName)
	mtlDesc.SetDiffuseColor(clr)
	mtlDesc.SetDiffuseMapFilename("")
	return (mtlDesc)

class T3DSFileSaver:
	def __CalculateMaterialName(self, mat):
		matName = (get_material_name(mat))
		
		if((matName)[0:2] == "P:"):
			return (matName)

		texFileName = (get_diffuse_map_filename(mat))
		if(texFileName == ""):
			return (matName)

		indexTex = ( -1)
		for i in range((0), (self. __matRenParams.GetNumMaterials())):
			if(stricmp(texFileName, self. __matRenParams.GetTexture(i)) == 0):
				indexTex = (i)
				break

		if(indexTex != -1):
			indexTexAndMat = ( -1)
			for i in range((0), (self. __matRenParams.GetNumMaterials())):
				if(stricmp(matName, self. __matRenParams.GetName(i)) == 0):
					if(stricmp(texFileName, self. __matRenParams.GetTexture(i)) == 0):
						indexTexAndMat = (i)
						break

			if(indexTexAndMat != -1):
				return (matName)
			else:
				return (self. __matRenParams.GetName(indexTex))

		if(self. __matRenParams.GetAutoNames()):
			matName = (texFileName)	
			
			pos = (len(matName) - 1)
			while(pos >= 0):
				if((matName)[pos:pos + 1] == "."):
					matName = ((matName)[0:pos])
					break
				
				pos = (pos - 1)

			pos = (len(matName) - 1)
			while(pos >= 0):
				ch = ((matName)[pos:pos + 1])
				if(ch == "\\" or ch == "/"):
					matName = ((matName)[pos + 1:len(matName)])
					break
				
				pos = (pos - 1)

		return (matName)

	def __EnumObjects(self, obj):
		if(obj != None):
			index = ( -1)
			for i in range((0), ((len(self. __objectsNames)))):
				if(((get_object_name(obj)) == ((self. __objectsNames)[i])) and is_mesh_object(obj)):
					index = (i)
					break

			if(index != -1):
				caption = ("Enumerating objects")
				show_progress_bar(caption, index * 100 / (len(self. __objectsNames)))
				
				self. __expObjects.append(obj)
				msh = (MeshData(obj))
				self. __expMeshes.append(msh)
				
				expMtlsForThisObj = ([])
				addMtlsForThisObj = ([])
				
				numFaces = ((msh.get_num_faces()))
				for i in range((0), (numFaces)):
					mat = ((msh.get_face_material(i)))
					if(mat == None):
						if((len(addMtlsForThisObj)) == 0):
							addMtlsForThisObj.append(GetDefaultMaterial(obj))
						
					else:
						alreadyInList = (False)
						for j in range((0), ((len(expMtlsForThisObj)))):
							if(((expMtlsForThisObj)[j]) == mat):
								alreadyInList = (True)
								break

						if( not (alreadyInList)):
							expMtlsForThisObj.append(mat)

				self. __expMtlsForEachObj.append(expMtlsForThisObj)
				self. __addMtlsForEachObj.append(addMtlsForThisObj)

		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			self. __EnumObjects(((children)[i]))

	def __MakeTotalListOfMaterials(self):
		for i in range((0), ((len(self. __expMtlsForEachObj)))):
			caption = ("Making total list of materials")
			show_progress_bar(caption, i * 90 / (len(self. __expMtlsForEachObj)))
			for j in range((0), ((len(((self. __expMtlsForEachObj)[i]))))):
				mat = (((((self. __expMtlsForEachObj)[i]))[j]))
				alreadyInList = (False)
				for k in range((0), ((len(self. __expMtls)))):
					if(((self. __expMtls)[k]) == mat):
						alreadyInList = (True)
						break

				if( not (alreadyInList)):
					self. __expMtls.append(mat)

		for i in range((0), ((len(self. __addMtlsForEachObj)))):
			caption = ("Making total list of materials")
			show_progress_bar(caption, 90 + i * 10 / (len(self. __addMtlsForEachObj)))
			for j in range((0), ((len(((self. __addMtlsForEachObj)[i]))))):
				matDesc = (((((self. __addMtlsForEachObj)[i]))[j]))
				alreadyInList = (False)
				for k in range((0), ((len(self. __addMtls)))):
					if(((self. __addMtls)[k]).GetMaterialName() == matDesc.GetMaterialName()):
						alreadyInList = (True)
						break

				if( not (alreadyInList)):
					self. __addMtls.append(matDesc)

	def __Write3DSVersion(self, version3DS):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x0002)
		self. __file.WriteUnsignedLong(version3DS)
		chunk.WriteEnd(self. __file)

	def __WriteMeshVersion(self, meshVersion):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x3D3E)
		self. __file.WriteUnsignedLong(meshVersion)
		chunk.WriteEnd(self. __file)

	def __WriteColor(self, colorChunkID, clr):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, colorChunkID)
		subchunk = (New3DSWriteChunk())
		subchunk.WriteBegin(self. __file, 0x0011)
		self. __file.WriteUnsignedChar((float_to_int(get_red(clr) * 255)))
		self. __file.WriteUnsignedChar((float_to_int(get_green(clr) * 255)))
		self. __file.WriteUnsignedChar((float_to_int(get_blue(clr) * 255)))
		subchunk.WriteEnd(self. __file)
		chunk.WriteEnd(self. __file)

	def __WriteMap(self, mapChunkID, mapName):
		if(mapName == ""):
			return
		
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, mapChunkID)
		subchunk = (New3DSWriteChunk())
		subchunk.WriteBegin(self. __file, 0xA300)
		self. __file.WriteString(mapName)
		subchunk.WriteEnd(self. __file)
		chunk.WriteEnd(self. __file)	

	def __WriteMaterialBlock(self, mtlDesc):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0xAFFF)
		
		subchunk = (New3DSWriteChunk())
		subchunk.WriteBegin(self. __file, 0xA000)
		self. __file.WriteString(mtlDesc.GetMaterialName())
		subchunk.WriteEnd(self. __file)
		
		self. __WriteColor(0xA020, mtlDesc.GetDiffuseColor())
		self. __WriteMap(0xA200, mtlDesc.GetDiffuseMapFilename())
		chunk.WriteEnd(self. __file)

	def __WriteOneUnit(self, oneUnit):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x0100)
		self. __file.WriteFloat(oneUnit)
		chunk.WriteEnd(self. __file)

	def __WriteVerticesList(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4110)
		
		transform = ((get_transform(self. __curObj)))
		if(self. __useLocalCS):
			transform = (multiply_matrix_matrix(transform, inverse_matrix((get_transform(self. __curObj)))))

		self. __curObjTMdet = (determinant(transform))
		
		numVerts = ((len(self. __curObjVerts)))
		self. __file.WriteUnsignedShort(numVerts)
		
		caption = (FormatMsg1("Writing '%1'", self. __curObjName))
		for i in range((0), (numVerts)):
			if(((i) & (255)) == 0):
				show_progress_bar(caption, (50 + 10 * i / numVerts + 100 * self. __curObjIndex) / (len(self. __objectsNames)))

			pt = (((self. __curObjVerts)[i]))
			
			pt = (multiply_vector_matrix(pt, transform))
			
			self. __file.WriteFloat(get_x(pt) * self. __scaleCoef)
			self. __file.WriteFloat(get_y(pt) * self. __scaleCoef)
			self. __file.WriteFloat(get_z(pt) * self. __scaleCoef)
		
		chunk.WriteEnd(self. __file)

	def __WriteFacesMtlList(self, mat, matName):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4130)
		self. __file.WriteString(matName)
		entries = ([])
		for i in range((0), ((self. __curObjMesh.get_num_faces()))):
			if((self. __curObjMesh.get_face_material(i)) == mat):
				entries.append(i)

		numEntries = ((len(entries)))
		self. __file.WriteUnsignedShort(numEntries)
		for j in range((0), (numEntries)):
			self. __file.WriteUnsignedShort(((entries)[j]))
		
		chunk.WriteEnd(self. __file)

	def __WriteSmoothGroupList(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4150)
		numFaces = ((self. __curObjMesh.get_num_faces()))
		for i in range((0), (numFaces)):
			self. __file.WriteUnsignedLong(0)
		
		chunk.WriteEnd(self. __file)

	def __WriteMappingCoords(self):
		if((self. __curObjMesh.get_num_tverts()) == 0):
			return

		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4140)
		numTVerts = ((len(self. __curObjTVerts)))
		self. __file.WriteUnsignedShort(numTVerts)
		caption = (FormatMsg1("Writing '%1'", self. __curObjName))
		for i in range((0), (numTVerts)):
			if(((i) & (255)) == 0):
				show_progress_bar(caption, (60 + 15 * i / numTVerts + 100 * self. __curObjIndex) / (len(self. __objectsNames)))

			uvvert = (((self. __curObjTVerts)[i]))
			self. __file.WriteFloat(get_u(uvvert))
			self. __file.WriteFloat(get_v(uvvert))
		
		chunk.WriteEnd(self. __file)

	def __WriteFacesDescription(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4120)
		
		numFaces = ((len(self. __curObjFaces)))
		self. __file.WriteUnsignedShort(numFaces)
		caption = (FormatMsg1("Writing '%1'", self. __curObjName))
		
		for i in range((0), (numFaces)):
			if(((i) & (127)) == 0):
				show_progress_bar(caption, (75 + 25 * i / numFaces + 100 * self. __curObjIndex) / (len(self. __objectsNames)))

			face = (((self. __curObjFaces)[i]))
			v0 = (get_face_vert(face, 0))
			v1 = (get_face_vert(face, 1))
			v2 = (get_face_vert(face, 2))
			visAB = (True)
			visBC = (True)
			visCA = (True)
			
			if(self. __curObjTMdet < 0):
				tmp = (v0)
				v0 = (v2)
				v2 = (tmp)
				tmp2 = (visAB)
				visAB = (visBC)
				visBC = (tmp2)

			flags = (0)
			if(visCA):
				flags = (((flags) | (0x01)))
			
			if(visBC):
				flags = (((flags) | (0x02)))
			
			if(visAB):
				flags = (((flags) | (0x04)))

			self. __file.WriteUnsignedShort(v0)
			self. __file.WriteUnsignedShort(v1)
			self. __file.WriteUnsignedShort(v2)
			self. __file.WriteUnsignedShort(flags)

		for i in range((0), ((len(self. __curObjExpMtls)))):
			mat = (((self. __curObjExpMtls)[i]))
			matName = (self. __CalculateMaterialName(mat))
			self. __WriteFacesMtlList(mat, matName)

		for i in range((0), ((len(self. __curObjAddMtls)))):
			matDesc = (((self. __curObjAddMtls)[i]))
			matName = (matDesc.GetMaterialName())
			self. __WriteFacesMtlList(None, matName)

		self. __WriteSmoothGroupList()
		chunk.WriteEnd(self. __file)

	def __ConvertTo3DSFaces(self):
		self. __curObjFaces = ([])
		self. __curObjVerts = ([])
		self. __curObjTVerts = ([])
		dupl = ([])
		
		numFaces = ((self. __curObjMesh.get_num_faces()))
		numVerts = ((self. __curObjMesh.get_num_verts()))
		origNumVerts = (numVerts)
		
		if((self. __curObjMesh.get_num_tverts()) == 0):
			for i in range((0), (numVerts)):
				self. __curObjVerts.append((self. __curObjMesh.get_vert(i)))
			
			for i in range((0), (numFaces)):
				self. __curObjFaces.append((self. __curObjMesh.get_face(i)))
			
			return

		for k in range((0), (origNumVerts)):
			self. __curObjVerts.append(new_vector(0, 0, 0))
			self. __curObjTVerts.append(new_uvvert(0, 0))
			dupl.append([])

		caption = (FormatMsg1("Writing '%1'", self. __curObjName))
		for i in range((0), (numFaces)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, (50 * i / numFaces + 100 * self. __curObjIndex) / (len(self. __objectsNames)))

			face = ((self. __curObjMesh.get_face(i)))
			tvFace = ((self. __curObjMesh.get_tvface(i)))
			
			for j in range((0), (3)):
				vIndex = (get_face_vert(face, j))
				vert = ((self. __curObjMesh.get_vert(vIndex)))
				tvert = ((self. __curObjMesh.get_tvert(get_tvface_vert(tvFace, j))))
				
				d = (((dupl)[vIndex]))
				
				if((len(d)) == 0):
					self. __curObjVerts[vIndex] = (vert)
					self. __curObjTVerts[vIndex] = (tvert)
					d.append(vIndex)
				else:
					foundMatch = (False)
					for z in range((0), ((len(d)))):
						uv1 = (((self. __curObjTVerts)[((d)[z])]))
						if((get_u(uv1) == get_u(tvert)) and (get_v(uv1) == get_v(tvert))):
							set_face_vert(face, j, ((d)[z]))
							foundMatch = (True)
							break

					if( not (foundMatch)):
						set_face_vert(face, j, numVerts)
						d.append(numVerts)
						numVerts = (numVerts + 1)
						self. __curObjVerts.append(vert)
						self. __curObjTVerts.append(tvert)

				dupl[vIndex] = (d)

			self. __curObjFaces.append(face)

	def __WriteMesh(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4100)
		self. __WriteVerticesList()
		self. __WriteMappingCoords()
		self. __WriteFacesDescription()
		chunk.WriteEnd(self. __file)

	def __WriteObjectHidden(self):
		if((is_visible(self. __curObj))):
			return
		
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4010)
		chunk.WriteEnd(self. __file)

	def __WriteObjectBlock(self, i):
		self. __curObjIndex = (i)
		self. __curObj = (((self. __expObjects)[i]))
		self. __curObjName = ((get_object_name(self. __curObj)))
		self. __curObjMesh = (((self. __expMeshes)[i]))
		self. __curObjExpMtls = (((self. __expMtlsForEachObj)[i]))
		self. __curObjAddMtls = (((self. __addMtlsForEachObj)[i]))
		
		objst = (NewObjectStats())
		objst.SetNameInFile(self. __curObjName)
		objst.SetNameInScene(self. __curObjName)
		objst.SetNumMtls((len(self. __curObjExpMtls)) + (len(self. __curObjAddMtls)))
		objst.SetNumFaces((self. __curObjMesh.get_num_faces()))
		objst.SetNumVertsInScene((self. __curObjMesh.get_num_verts()))
		objst.SetNumVertsInFile((self. __curObjMesh.get_num_verts()))
		
		canBeSaved = (True)
		if((self. __curObjMesh.get_num_faces()) > 65535):
			canBeSaved = (False)
		elif((self. __curObjMesh.get_num_verts()) > 65535):
			canBeSaved = (False)
		else:
			self. __ConvertTo3DSFaces()
			objst.SetNumVertsInFile((len(self. __curObjVerts)))
			if((len(self. __curObjVerts)) > 65535):
				canBeSaved = (False)

		if(canBeSaved):
			chunk = (New3DSWriteChunk())
			chunk.WriteBegin(self. __file, 0x4000)
			self. __file.WriteString(self. __curObjName)
			self. __WriteObjectHidden()
			self. __WriteMesh()
			chunk.WriteEnd(self. __file)

		self. __expObjectStats.append(objst)	

	def __Write3DEditor(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x3D3D)
		self. __WriteMeshVersion(3)
		for i in range((0), ((len(self. __expMtls)))):
			mat = (((self. __expMtls)[i]))
			matDesc = (NewMaterialDesc())
			matDesc.SetMaterialName(self. __CalculateMaterialName(mat))
			matDesc.SetDiffuseColor(get_diffuse_color(mat))
			matDesc.SetDiffuseMapFilename(get_diffuse_map_filename(mat))
			caption = (FormatMsg1("Writing materials", matDesc.GetMaterialName()))
			show_progress_bar(caption, i)
			self. __WriteMaterialBlock(matDesc)
		
		for i in range((0), ((len(self. __addMtls)))):
			matDesc = (((self. __addMtls)[i]))
			caption = (FormatMsg1("Writing materials", matDesc.GetMaterialName()))
			show_progress_bar(caption, 95 + 5 * i / (len(self. __addMtls)))
			self. __WriteMaterialBlock(matDesc)
		
		self. __WriteOneUnit(1)
		for i in range((0), ((len(self. __expObjects)))):
			self. __WriteObjectBlock(i)
		
		chunk.WriteEnd(self. __file)

	def __WriteMainChunk(self):
		chunk = (New3DSWriteChunk())
		chunk.WriteBegin(self. __file, 0x4D4D)
		self. __Write3DSVersion(3)
		self. __Write3DEditor()
		chunk.WriteEnd(self. __file)

	def Init(self):
		self. __file = (NewFile())
		self. __scaleCoef = (1.0)
		self. __matRenParams = (NewMatRenParams())
		self. __objectsNames = ([])
		self. __useLocalCS = (True)
		self. __expObjects = ([])
		self. __expMeshes = ([])
		self. __expMtlsForEachObj = ([])
		self. __addMtlsForEachObj = ([])
		self. __expMtls = ([])
		self. __addMtls = ([])
		self. __expObjectStats = ([])
		self. __curObjIndex = ( -1)
		self. __curObj = (None)
		self. __curObjName = ("")
		self. __curObjExpMtls = ([])
		self. __curObjAddMtls = ([])
		self. __curObjTMdet = (0)
		self. __curObjMesh = (None)
		self. __curObjVerts = ([])
		self. __curObjFaces = ([])
		self. __curObjTVerts = ([])

	def Write3DSFile(self, filename, objectsNames, useLocalCS, spaceTransform, matRenParamsPath):
		self. Init()
		self. __objectsNames = (objectsNames)
		self. __useLocalCS = (useLocalCS)
		self. __scaleCoef = (spaceTransform.GetFileUnitsPerSystemUnit())
		self. __matRenParams.LoadTextFile(matRenParamsPath)
		try:
			self. __file.Open(filename, "wb")
			self. __EnumObjects(None)
			self. __MakeTotalListOfMaterials()
			self. __WriteMainChunk()
			self. __file.Close()
		
		except RuntimeError as ex:
			self. __file.Close()
			delete_file(filename)
			raise

	def GetObjectStats(self):
		return (self. __expObjectStats)

def New3DSFileSaver():
	saver = (T3DSFileSaver())
	saver.Init()
	return (saver)

class TNameAnalyzer:
	def Init(self):
		self. __fullName = ("")
		self. __prefix = ("")
		self. __shortName = ("")
		self. __objType = (0)

	def Analyze(self, fullName):
		self. Init()
		self. __fullName = (fullName)
		self. __shortName = (fullName)
		
		fullLen = (len(fullName))
		for i in range((0), (fullLen - 2)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "Z") or (s1 == "z")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "S") or (s2 == "s")):
					s3 = ((fullName)[i + 2:i + 3])
					if(s3 == "_"):
						self. __prefix = ((fullName)[0:i])
						self. __shortName = ((fullName)[i:fullLen])
						self. __objType = (2)
						return

		for i in range((0), (fullLen - 2)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "Z") or (s1 == "z")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "M") or (s2 == "m")):
					s3 = ((fullName)[i + 2:i + 3])
					if(s3 == "_"):
						self. __prefix = ((fullName)[0:i])
						self. __shortName = ((fullName)[i:fullLen])
						self. __objType = (3)
						return

		for i in range((0), (fullLen - 4)):
			s1 = ((fullName)[i:i + 1])
			if((s1 == "B") or (s1 == "b")):
				s2 = ((fullName)[i + 1:i + 2])
				if((s2 == "I") or (s2 == "i")):
					s3 = ((fullName)[i + 2:i + 3])
					if((s3 == "P") or (s3 == "p")):
						s4 = ((fullName)[i + 3:i + 4])
						if(s4 == "0"):
							s5 = ((fullName)[i + 4:i + 5])
							if(s5 == "1"):
								self. __prefix = ((fullName)[0:i])
								self. __shortName = ((fullName)[i:fullLen])
								self. __objType = (1)

	def GetObjectType(self):
		return (self. __objType)

	def GetFullName(self):
		return (self. __fullName)

	def GetShortName(self):
		return (self. __shortName)

	def GetPrefix(self):
		return (self. __prefix)

def NewNameAnalyzer():
	namAnl = (TNameAnalyzer())
	namAnl.Init()
	return (namAnl)

def AnalyzeName(fullName):
	namAnl = (NewNameAnalyzer())
	namAnl.Analyze(fullName)
	return (namAnl)

class TModelHierarchy:
	def Init(self):
		self. __modelPrefix = ("")
		self. __modelType = (0)
		self. __objects = ([])
		self. __objectParents = ([])
		self. __objectTypes = ([])

	def Write(self, f):
		f.WriteString(self. __modelPrefix)
		f.WriteUnsignedChar(self. __modelType)
		numObjects = ((len(self. __objects)))
		f.WriteUnsignedLong(numObjects)
		for i in range((0), (numObjects)):
			f.WriteString(((self. __objects)[i]))
			f.WriteString(((self. __objectParents)[i]))
			f.WriteUnsignedChar(((self. __objectTypes)[i]))

	def Read(self, f):
		self. __modelPrefix = (f.ReadString())
		self. __modelType = (f.ReadUnsignedChar())
		numObjects = (f.ReadUnsignedLong())
		self. __objects = ([])
		self. __objectParents = ([])
		self. __objectTypes = ([])
		for i in range((0), (numObjects)):
			self. __objects.append(f.ReadString())
			self. __objectParents.append(f.ReadString())
			self. __objectTypes.append(f.ReadUnsignedChar())

	def SetModelPrefix(self, modelPrefix):
		self. __modelPrefix = (modelPrefix)

	def GetModelPrefix(self):
		return (self. __modelPrefix)

	def SetModelType(self, modelType):
		self. __modelType = (modelType)

	def GetModelType(self):
		return (self. __modelType)

	def SetObjects(self, objects):
		self. __objects = (objects)

	def GetObjects(self):
		return (self. __objects)

	def SetObjectParents(self, objectParents):
		self. __objectParents = (objectParents)

	def GetObjectParents(self):
		return (self. __objectParents)

	def SetObjectTypes(self, objectTypes):
		self. __objectTypes = (objectTypes)

	def GetObjectTypes(self):
		return (self. __objectTypes)

def NewModelHierarchy():
	mh = (TModelHierarchy())
	mh.Init()
	return (mh)

class TMeshDesc:
	def Init(self):
		self. __verts = ([])
		self. __faces = ([])
		self. __faceMatIDs = ([])
		self. __faceSmoothGroups = ([])
		self. __edgeVis = ([])
		self. __tvFaces = ([])
		self. __tVerts = ([])

	def GetNumVerts(self):
		return ((len(self. __verts)))

	def SetNumVerts(self, numVerts):
		if(numVerts != (len(self. __verts))):
			self. __verts = ([])
			
			for j in range((0), (numVerts)):
				self. __verts.append(zero_vector())

	def GetVert(self, i):
		return (((self. __verts)[i]))

	def SetVert(self, i, pt):
		self. __verts[i] = (pt)

	def GetNumFaces(self):
		return ((len(self. __faces)))

	def SetNumFaces(self, numFaces):
		if(numFaces != (len(self. __faces))):
			self. __faces = ([])
			
			for j in range((0), (numFaces)):
				self. __faces.append(new_face(0, 0, 0))
			
			self. __faceMatIDs = ([])
			
			for j in range((0), (numFaces)):
				self. __faceMatIDs.append(0)
			
			self. __faceSmoothGroups = ([])
			
			for j in range((0), (numFaces)):
				self. __faceSmoothGroups.append(0)
			
			self. __edgeVis = ([])
			
			for j in range((0), (numFaces)):
				arr3 = ([])
				
				arr3.append(True)
				arr3.append(True)
				arr3.append(True)
				self. __edgeVis.append(arr3)

	def GetFace(self, faceIndex):
		return (((self. __faces)[faceIndex]))

	def SetFace(self, faceIndex, f):
		self. __faces[faceIndex] = (f)

	def GetFaceMatID(self, faceIndex):
		return (((self. __faceMatIDs)[faceIndex]))

	def SetFaceMatID(self, faceIndex, matID):
		self. __faceMatIDs[faceIndex] = (matID)

	def GetFaceSmoothGroup(self, faceIndex):
		return (((self. __faceSmoothGroups)[faceIndex]))

	def SetFaceSmoothGroup(self, faceIndex, smGroup):
		self. __faceSmoothGroups[faceIndex] = (smGroup)

	def GetEdgeVis(self, faceIndex, edgeIndex):
		return (((((self. __edgeVis)[faceIndex]))[edgeIndex]))

	def SetEdgeVis(self, faceIndex, edgeIndex, vis):
		((self. __edgeVis)[faceIndex])[edgeIndex] = (vis)

	def GetNumTVerts(self):
		return ((len(self. __tVerts)))

	def SetNumTVerts(self, numTVerts):
		if(numTVerts != (len(self. __tVerts))):
			self. __tVerts = ([])
			
			for j in range((0), (numTVerts)):
				self. __tVerts.append(zero_uvvert())

	def GetTVert(self, i):
		return (((self. __tVerts)[i]))

	def SetTVert(self, i, uv):
		self. __tVerts[i] = (uv)

	def GetNumTVFaces(self):
		return ((len(self. __tvFaces)))

	def SetNumTVFaces(self, numTVFaces):
		if(numTVFaces != (len(self. __tvFaces))):
			self. __tvFaces = ([])
			
			for j in range((0), (numTVFaces)):
				self. __tvFaces.append(new_tvface(0, 0, 0))

	def GetTVFace(self, i):
		return (((self. __tvFaces)[i]))

	def SetTVFace(self, i, tf):
		self. __tvFaces[i] = (tf)

def NewMeshDesc():
	m = (TMeshDesc())
	m.Init()
	return (m)

class TSoftSkinVert:
	def Init(self):
		self. __bones = ([])
		self. __weights = ([])

	def SetNumWeights(self, numWeights):
		if((len(self. __bones)) != numWeights):
			self. __bones = ([])
			self. __weights = ([])
			for i in range((0), (numWeights)):
				self. __bones.append("")
				self. __weights.append(0)

	def GetNumWeights(self):
		return ((len(self. __bones)))

	def SetBoneName(self, vertBoneIndex, boneName):
		self. __bones[vertBoneIndex] = (boneName)

	def GetBoneName(self, vertBoneIndex):
		return (((self. __bones)[vertBoneIndex]))

	def SetWeight(self, vertBoneIndex, weight):
		self. __weights[vertBoneIndex] = (weight)

	def GetWeight(self, vertBoneIndex):
		return (((self. __weights)[vertBoneIndex]))

def NewSoftSkinVert():
	sv = (TSoftSkinVert())
	sv.Init()
	return (sv)

class TSoftSkinVerts:
	def Init(self):
		self. __verts = ([])

	def SetNumVerts(self, numVerts):
		if((len(self. __verts)) != numVerts):
			self. __verts = ([])
			for i in range((0), (numVerts)):
				self. __verts.append(NewSoftSkinVert())

	def GetNumVerts(self):
		return ((len(self. __verts)))

	def SetVert(self, vertIndex, sv):
		self. __verts[vertIndex] = (sv)

	def GetVert(self, vertIndex):
		return (((self. __verts)[vertIndex]))

def NewSoftSkinVerts():
	svs = (TSoftSkinVerts())
	svs.Init()
	return (svs)

class TPosTrack:
	def Init(self):
		self. __samplePositions = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __samplePositions)) != numSamples):
			self. __samplePositions = ([])
			for i in range((0), (numSamples)):
				self. __samplePositions.append(zero_vector())

	def GetNumSamples(self):
		return ((len(self. __samplePositions)))

	def SetSamplePos(self, sampleIdx, pos):
		self. __samplePositions[sampleIdx] = (pos)

	def GetSamplePos(self, sampleIdx):
		return (((self. __samplePositions)[sampleIdx]))

def NewPosTrack():
	tr = (TPosTrack())
	tr.Init()
	return (tr)

class TRotTrack:
	def Init(self):
		self. __sampleAxes = ([])
		self. __sampleAngles = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __sampleAxes)) != numSamples):
			self. __sampleAxes = ([])
			self. __sampleAngles = ([])
			for i in range((0), (numSamples)):
				self. __sampleAxes.append(zero_vector())
				self. __sampleAngles.append(0)

	def GetNumSamples(self):
		return ((len(self. __sampleAxes)))

	def SetSampleAxis(self, sampleIdx, axis):
		self. __sampleAxes[sampleIdx] = (axis)

	def GetSampleAxis(self, sampleIdx):
		return (((self. __sampleAxes)[sampleIdx]))

	def SetSampleAngle(self, sampleIdx, angle):
		self. __sampleAngles[sampleIdx] = (angle)

	def GetSampleAngle(self, sampleIdx):
		return (((self. __sampleAngles)[sampleIdx]))

def NewRotTrack():
	tr = (TRotTrack())
	tr.Init()
	return (tr)

class TMorphTrack:
	def Init(self):
		self. __sampleVerts = ([])

	def SetNumSamples(self, numSamples):
		if((len(self. __sampleVerts)) != numSamples):
			self. __sampleVerts = ([])
			for j in range((0), (numSamples)):
				self. __sampleVerts.append([])

	def GetNumSamples(self):
		return ((len(self. __sampleVerts)))

	def SetSampleVerts(self, sampleIdx, verts):
		self. __sampleVerts[sampleIdx] = (verts)

	def GetSampleVerts(self, sampleIdx):
		return (((self. __sampleVerts)[sampleIdx]))

def NewMorphTrack():
	tr = (TMorphTrack())
	tr.Init()
	return (tr)

class TObjectDesc:
	def Init(self):
		self. __objectName = ("")
		self. __objectType = (0)
		self. __parentName = ("")
		self. __transform = (identity_matrix())
		self. __matRef = (0)
		self. __meshDesc = (NewMeshDesc())
		self. __ssv = (NewSoftSkinVerts())
		self. __posTrack = (NewPosTrack())
		self. __rotTrack = (NewRotTrack())
		self. __morphTrack = (NewMorphTrack())

	def GetObjectName(self):
		return (self. __objectName)

	def SetObjectName(self, objectName):
		self. __objectName = (objectName)

	def GetObjectType(self):
		return (self. __objectType)

	def SetObjectType(self, objectType):
		self. __objectType = (objectType)

	def GetParentName(self):
		return (self. __parentName)

	def SetParentName(self, parentName):
		self. __parentName = (parentName)

	def GetTransform(self):
		a00 = (get_x(get_row0(self. __transform)))
		a01 = (get_y(get_row0(self. __transform)))
		a02 = (get_z(get_row0(self. __transform)))
		a10 = (get_x(get_row1(self. __transform)))
		a11 = (get_y(get_row1(self. __transform)))
		a12 = (get_z(get_row1(self. __transform)))
		a20 = (get_x(get_row2(self. __transform)))
		a21 = (get_y(get_row2(self. __transform)))
		a22 = (get_z(get_row2(self. __transform)))
		a30 = (get_x(get_row3(self. __transform)))
		a31 = (get_y(get_row3(self. __transform)))
		a32 = (get_z(get_row3(self. __transform)))
		transformCopy = (new_matrix(new_vector(a00, a01, a02), new_vector(a10, a11, a12), new_vector(a20, a21, a22), new_vector(a30, a31, a32)))
		return (transformCopy)

	def SetTransform(self, transform):
		self. __transform = (transform)

	def GetMaterialRef(self):
		return (self. __matRef)

	def SetMaterialRef(self, matRef):
		self. __matRef = (matRef)

	def GetMeshDesc(self):
		return (self. __meshDesc)

	def SetMeshDesc(self, meshDesc):
		self. __meshDesc = (meshDesc)

	def GetPosTrack(self):
		return (self. __posTrack)

	def SetPosTrack(self, posTrack):
		self. __posTrack = (posTrack)

	def GetRotTrack(self):
		return (self. __rotTrack)

	def SetRotTrack(self, rotTrack):
		self. __rotTrack = (rotTrack)

	def GetMorphTrack(self):
		return (self. __morphTrack)

	def SetMorphTrack(self, morphTrack):
		self. __morphTrack = (morphTrack)

	def GetSoftSkinVerts(self):
		return (self. __ssv)

	def SetSoftSkinVerts(self, ssv):
		self. __ssv = (ssv)

def NewObjectDesc():
	m = (TObjectDesc())
	m.Init()
	return (m)

class TSceneAnalyzer:
	def __AppendPrefix(self, prefix, sel):
		found = (False)
		for i in range((0), ((len(self. __scenePrefixes)))):
			if(stricmp(((self. __scenePrefixes)[i]), prefix) == 0):
				found = (True)

		if( not (found)):
			self. __scenePrefixes.append(prefix)
		
		if(sel):
			found = (False)
			for i in range((0), ((len(self. __selectedPrefixes)))):
				if(stricmp(((self. __selectedPrefixes)[i]), prefix) == 0):
					found = (True)

			if( not (found)):
				self. __selectedPrefixes.append(prefix)

	def __PrepareSceneObjects(self, obj):
		if(obj != None):
			objName = ((get_object_name(obj)))
			selected = ((is_selected(obj)))
			isMeshByType = (is_mesh_object(obj))
			na = (AnalyzeName(objName))
			self. __sceneObjects.append(objName)
			if(selected):
				self. __selectedObjects.append(objName)
			
			if(isMeshByType):
				self. __sceneMeshesByType.append(objName)
				if(selected):
					self. __selectedMeshesByType.append(objName)

			prefix = (na.GetPrefix())
			objType = (na.GetObjectType())
			if(objType == 2):
				self. __sceneSlots.append(objName)
				if(selected):
					self. __selectedSlots.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			elif(objType == 1):
				self. __sceneBones.append(objName)
				if(selected):
					self. __selectedBones.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			elif(objType == 3):
				self. __sceneMeshes.append(objName)
				if(selected):
					self. __selectedMeshes.append(objName)
				
				self. __AppendPrefix(prefix, selected)
			else:
				self. __sceneDummies.append(objName)
				if(selected):
					self. __selectedDummies.append(objName)

		children = ((get_children(obj)))
		for i in range((0), ((len(children)))):
			self. __PrepareSceneObjects(((children)[i]))

	def __GetObjectsByPrefix(self, prefix):
		self. __objectsByPrefix = ([])
		
		conflictPrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			a = (((self. __scenePrefixes)[i]))
			if((a != prefix) and ((a)[0:len(prefix)] == prefix)):
				conflictPrefixes.append(a)

		for i in range((0), ((len(self. __sceneObjects)))):
			objName = (((self. __sceneObjects)[i]))
			if((objName)[0:len(prefix)] == prefix):
				noConflicts = (True)
				for j in range((0), ((len(conflictPrefixes)))):
					c = (((conflictPrefixes)[j]))
					if((objName)[0:len(c)] == c):
						noConflicts = (False)
						break

				if(noConflicts):
					self. __objectsByPrefix.append(objName)

		return (self. __objectsByPrefix)

	def __PrepareModelHierarchies(self):
		self. __modelHierarchies = ([])
		
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			objectsByPrefix = (self. __GetObjectsByPrefix(prefix))
			
			numBones = (0)
			numSlots = (0)
			numMeshes = (0)
			bip01Found = (False)
			objects = (objectsByPrefix)
			objectTypes = ([])
			for j in range((0), ((len(objects)))):
				objName = (((objects)[j]))
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if(objType == 1):
					numBones = (numBones + 1)
					if(stricmp(na.GetShortName(), "Bip01") == 0):
						bip01Found = (True)
					
				elif(objType == 2):
					numSlots = (numSlots + 1)
				elif(objType == 3):
					numMeshes = (numMeshes + 1)
				
				objects[j] = ((objName)[len(prefix):len(objName)])
				objectTypes.append(objType)

			modelType = (0)
			if(numMeshes == 1 and numBones == 0 and numSlots == 0):
				modelType = ((1 + 2))
			elif(bip01Found):
				modelType = ((8 + 16))
			else:
				modelType = ((4))

			if(modelType == (8 + 16)):
				rootBoneIndex = ( -1)
				for i in range((0), ((len(objects)))):
					objName = (((objects)[i]))
					objType = (((objectTypes)[i]))
					if(objType == 1):
						rootBoneIndex = (i)
						break

				if(rootBoneIndex != -1 and rootBoneIndex != 0):
					tmp = (((objects)[0]))
					objects[0] = (((objects)[rootBoneIndex]))
					objects[rootBoneIndex] = (tmp)
					tmp2 = (((objectTypes)[0]))
					objectTypes[0] = (((objectTypes)[rootBoneIndex]))
					objectTypes[rootBoneIndex] = (tmp2)
				
				withoutLeadDummies = ([])
				withoutLeadDummies2 = ([])
				for i in range((0), ((len(objects)))):
					objName = (((objects)[i]))
					objType = (((objectTypes)[i]))
					if((objType != 0) or (i > rootBoneIndex)):
						withoutLeadDummies.append(objName)
						withoutLeadDummies2.append(objType)

				objects = (withoutLeadDummies)
				objectTypes = (withoutLeadDummies2)

			objectParents = ([])
			for j in range((0), ((len(objects)))):
				defaultParentName = ("")
				if((modelType == (8 + 16)) and (j > 0)):
					defaultParentName = (((objects)[0]))
				
				objName = (((objects)[j]))
				obj = ((find_object_by_name(prefix + objName)))
				parentName = (defaultParentName)
				if(obj != None):
					parent = ((get_parent(obj)))
					if(parent != None):
						tmp = ((get_object_name(parent)))
						for k in range((0), (j)):
							if(tmp == prefix + ((objects)[k])):
								parentName = (((objects)[k]))
								break

				objectParents.append(parentName)

			mh = (NewModelHierarchy())
			mh.SetModelPrefix(prefix)
			mh.SetModelType(modelType)
			mh.SetObjects(objects)
			mh.SetObjectParents(objectParents)
			mh.SetObjectTypes(objectTypes)
			self. __modelHierarchies.append(mh)

	def __GetModelHierarchyByPrefix(self, prefix):
		for i in range((0), ((len(self. __modelHierarchies)))):
			mh = (((self. __modelHierarchies)[i]))
			if(mh.GetModelPrefix() == prefix):
				return (mh)

		return (((self. __modelHierarchies)[ -1]))

	def __CheckPrefixForUnique(self, prefix):
		for i in range((0), ((len(self. __scenePrefixes)))):
			if(((self. __scenePrefixes)[i]) == prefix):
				return (False)

		return (True)

	def __PrepareUniquePrefix(self):
		prefix = ("")
		a = ((ord("A")))
		z = ((ord("Z")))
		codes = ([])
		
		while( not (self. __CheckPrefixForUnique(prefix))):
			pos = ((len(codes)) - 1)
			while(True):
				if(pos == -1):
					for i in range((0), ((len(codes)))):
						codes[i] = (a)
					
					codes.append(a)
					break
				elif(((codes)[pos]) < z):
					codes[pos] = (((codes)[pos]) + 1)
					break
				else:
					codes[pos] = (a)
					pos = (pos - 1)

			prefix = ("")
			for i in range((0), ((len(codes)))):
				prefix = (prefix + (chr(((codes)[i]))))
			
			if(len(prefix) != 0):
				prefix = (prefix + " ")

		self. __uniquePrefix = (prefix)

	def __Init(self):
		self. __sceneObjects = ([])
		self. __selectedObjects = ([])
		self. __sceneMeshesByType = ([])
		self. __selectedMeshesByType = ([])
		self. __sceneSlots = ([])
		self. __selectedSlots = ([])
		self. __sceneBones = ([])
		self. __selectedBones = ([])
		self. __sceneMeshes = ([])
		self. __selectedMeshes = ([])
		self. __sceneDummies = ([])
		self. __selectedDummies = ([])
		self. __scenePrefixes = ([])
		self. __selectedPrefixes = ([])
		self. __uniquePrefix = ("")
		self. __objectsByPrefix = ([])
		self. __modelHierarchies = ([])
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])

	def __Analyze(self):
		show_progress_bar("Analyzing scene", 1)
		self. __Init()
		self. __PrepareSceneObjects(None)
		self. __PrepareModelHierarchies()
		self. __PrepareUniquePrefix()

	def __FindAppropriateDynamicModels(self, objectsDesc):
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			appropriate = (True)
			
			for j in range((0), ((len(objectsDesc)))):
				objName = (((objectsDesc)[j]).GetObjectName())
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if((objType == 1) or (objType == 2)):
					matchFound = (False)
					if(objType == 1):
						for k in range((0), ((len(self. __sceneBones)))):
							if(stricmp(((self. __sceneBones)[k]), prefix + objName) == 0):
								matchFound = (True)
								break

					else:
						for k in range((0), ((len(self. __sceneSlots)))):
							if(stricmp(((self. __sceneSlots)[k]), prefix + objName) == 0):
								matchFound = (True)
								break

					if( not (matchFound)):
						appropriate = (False)
						break

			if(appropriate):
				self. __appropriatePrefixes.append(prefix)
				for j in range((0), ((len(self. __selectedPrefixes)))):
					if(((self. __selectedPrefixes)[j]) == prefix):
						self. __selectedAppropriatePrefixes.append(prefix)
						break

	def __FindAppropriateMorphMeshes(self, objectsDesc):
		self. __appropriatePrefixes = ([])
		self. __selectedAppropriatePrefixes = ([])
		for i in range((0), ((len(self. __scenePrefixes)))):
			prefix = (((self. __scenePrefixes)[i]))
			appropriate = (True)
			
			for j in range((0), ((len(objectsDesc)))):
				objName = (((objectsDesc)[j]).GetObjectName())
				na = (AnalyzeName(objName))
				objType = (na.GetObjectType())
				if(objType == 3):
					matchFound = (False)
					for k in range((0), ((len(self. __sceneMeshes)))):
						if(((self. __sceneMeshes)[k]) == prefix + objName):
							obj = ((find_object_by_name(prefix + objName)))
							if(is_mesh_object(obj)):
								msh = (MeshData(obj))
								numVerts1 = ((msh.get_num_verts()))
								morphTrack = (((objectsDesc)[j]).GetMorphTrack())
								if(morphTrack.GetNumSamples() > 0):
									numVerts2 = ((len(morphTrack.GetSampleVerts(0))))
									if(numVerts1 == numVerts2):
										matchFound = (True)
										break

					if( not (matchFound)):
						appropriate = (False)
						break

			if(appropriate):
				self. __appropriatePrefixes.append(prefix)
				for j in range((0), ((len(self. __selectedPrefixes)))):
					if(((self. __selectedPrefixes)[j]) == prefix):
						self. __selectedAppropriatePrefixes.append(prefix)
						break

	def Init(self):
		self. __Init()

	def Analyze(self):
		self. __Analyze()

	def GetSceneObjects(self):
		return (self. __sceneObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def GetSceneMeshesByType(self):
		return (self. __sceneMeshesByType)

	def GetSelectedMeshesByType(self):
		return (self. __selectedMeshesByType)

	def GetSceneSlots(self):
		return (self. __sceneSlots)

	def GetSelectedSlots(self):
		return (self. __selectedSlots)

	def GetSceneBones(self):
		return (self. __sceneBones)

	def GetSelectedBones(self):
		return (self. __selectedBones)

	def GetSceneMeshes(self):
		return (self. __sceneMeshes)

	def GetSelectedMeshes(self):
		return (self. __selectedMeshes)

	def GetSceneDummies(self):
		return (self. __sceneDummies)

	def GetSelectedDummies(self):
		return (self. __selectedDummies)

	def GetScenePrefixes(self):
		return (self. __scenePrefixes)

	def GetSelectedPrefixes(self):
		return (self. __selectedPrefixes)

	def GetUniquePrefix(self):
		return (self. __uniquePrefix)

	def GetModelHierarchies(self):
		return (self. __modelHierarchies)

	def GetModelHierarchyByPrefix(self, prefix):
		return (self. __GetModelHierarchyByPrefix(prefix))

	def FindAppropriateDynamicModels(self, objectsDesc):
		self. __FindAppropriateDynamicModels(objectsDesc)

	def FindAppropriateMorphMeshes(self, objectsDesc):
		self. __FindAppropriateMorphMeshes(objectsDesc)

	def GetAppropriatePrefixes(self):
		return (self. __appropriatePrefixes)

	def GetSelectedAppropriatePrefixes(self):
		return (self. __selectedAppropriatePrefixes)

def NewSceneAnalyzer():
	sa = (TSceneAnalyzer())
	sa.Init()
	return (sa)

def AnalyzeScene():
	sa = (NewSceneAnalyzer())
	sa.Analyze()
	return (sa)

class T3DSExporterDlgInput:
	def Init(self):
		self. __exportFileName = ("")
		self. __sceneObjects = ([])
		self. __selectedObjects = ([])
		self. __useLocalCS = (True)
		self. __fileUnitsPerSystemUnit = (1)

	def Write(self, f):
		f.WriteString(self. __exportFileName)
		f.WriteUnsignedLong((len(self. __sceneObjects)))
		for i in range((0), ((len(self. __sceneObjects)))):
			f.WriteString(((self. __sceneObjects)[i]))

		f.WriteUnsignedLong((len(self. __selectedObjects)))
		for i in range((0), ((len(self. __selectedObjects)))):
			f.WriteString(((self. __selectedObjects)[i]))
		
		f.WriteBool(self. __useLocalCS)
		f.WriteFloat(self. __fileUnitsPerSystemUnit)

	def Read(self, f):
		self. __exportFileName = (f.ReadString())
		
		numSceneObjects = (f.ReadUnsignedLong())
		self. __sceneObjects = ([])
		for i in range((0), (numSceneObjects)):
			self. __sceneObjects.append(f.ReadString())

		numSelectedObjects = (f.ReadUnsignedLong())
		self. __selectedObjects = ([])
		for i in range((0), (numSelectedObjects)):
			self. __selectedObjects.append(f.ReadString())
		
		self. __useLocalCS = (f.ReadBool())
		self. __fileUnitsPerSystemUnit = (f.ReadFloat())

	def SetExportFileName(self, exportFileName):
		self. __exportFileName = (exportFileName)

	def GetExportFileName(self):
		return (self. __exportFileName)

	def SetSceneObjects(self, sceneObjects):
		self. __sceneObjects = (sceneObjects)

	def GetSceneObjects(self):
		return (self. __sceneObjects)

	def SetSelectedObjects(self, selectedObjects):
		self. __selectedObjects = (selectedObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def SetUseLocalCS(self, useLocalCS):
		self. __useLocalCS = (useLocalCS)

	def GetUseLocalCS(self):
		return (self. __useLocalCS)

	def SetFileUnitsPerSystemUnit(self, scaleCoef):
		self. __fileUnitsPerSystemUnit = (scaleCoef)

	def GetFileUnitsPerSystemUnit(self):
		return (self. __fileUnitsPerSystemUnit)

def New3DSExporterDlgInput():
	dlgInput = (T3DSExporterDlgInput())
	dlgInput.Init()
	return (dlgInput)

class T3DSExporterDlgOutput:
	def Init(self, dlgInput):
		self. __selectedObjects = (dlgInput.GetSelectedObjects())
		self. __useLocalCS = (dlgInput.GetUseLocalCS())
		self. __spaceTransform = (NewSpaceTransform())
		self. __spaceTransform.SetFileUnitsPerSystemUnit(dlgInput.GetFileUnitsPerSystemUnit())
		self. __matRenParamsPath = ("")
		self. __continueExport = (True)

	def Write(self, f):
		f.WriteUnsignedLong((len(self. __selectedObjects)))
		for i in range((0), ((len(self. __selectedObjects)))):
			f.WriteString(((self. __selectedObjects)[i]))
		
		f.WriteBool(self. __useLocalCS)
		self. __spaceTransform.Write(f)
		f.WriteString(self. __matRenParamsPath)
		f.WriteBool(self. __continueExport)

	def Read(self, f):
		numSelectedObjects = (f.ReadUnsignedLong())
		self. __selectedObjects = ([])
		for i in range((0), (numSelectedObjects)):
			self. __selectedObjects.append(f.ReadString())
		
		self. __useLocalCS = (f.ReadBool())
		self. __spaceTransform.Read(f)
		self. __matRenParamsPath = (f.ReadString())
		self. __continueExport = (f.ReadBool())

	def SetSelectedObjects(self, selectedObjects):
		self. __selectedObjects = (selectedObjects)

	def GetSelectedObjects(self):
		return (self. __selectedObjects)

	def SetUseLocalCS(self, useLocalCS):
		self. __useLocalCS = (useLocalCS)

	def GetUseLocalCS(self):
		return (self. __useLocalCS)

	def SetSpaceTransform(self, spaceTransform):
		self. __spaceTransform = (spaceTransform)

	def GetSpaceTransform(self):
		return (self. __spaceTransform)

	def SetMatRenParamsPath(self, path):
		self. __matRenParamsPath = (path)

	def GetMatRenParamsPath(self):
		return (self. __matRenParamsPath)

	def SetContinueExport(self, continueExport):
		self. __continueExport = (continueExport)

	def GetContinueExport(self):
		return (self. __continueExport)

def New3DSExporterDlgOutput(dlgInput):
	dlgOutput = (T3DSExporterDlgOutput())
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
	register_exporter("Krx3dsExp", "3DS", "Kerrax 3D Studio Mesh") 

def unregister(): 
	unregister_exporter("Krx3dsExp") 

if __name__ == "main": 
	register() 

def Krx3dsExp(filename_param, quiet_param = False): 
	QUIET = quiet_param 
	EXPORT_FILE_NAME = filename_param
	
	sceneAnalyzer = (AnalyzeScene())
	
	dlgInput = (New3DSExporterDlgInput())
	dlgInput.SetExportFileName(EXPORT_FILE_NAME)
	dlgInput.SetSceneObjects(sceneAnalyzer.GetSceneMeshesByType())
	selectedObjects = (sceneAnalyzer.GetSelectedMeshesByType())
	if((len(selectedObjects)) == 0):
		selectedObjects = (sceneAnalyzer.GetSceneMeshesByType())
	
	dlgInput.SetSelectedObjects(selectedObjects)
	dlgInput.SetUseLocalCS((len(selectedObjects)) <= 1)
	dlgInput.SetFileUnitsPerSystemUnit(100)
	
	dlgOutput = (New3DSExporterDlgOutput(dlgInput))
	inputFile = (NewFile())
	outputFile = (NewFile())
	if( not (QUIET)):
		try:
			show_progress_bar("Showing dialog", 0)
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("3DSExporterDlgInput")
			dlgInput.Write(inputFile)
			inputFile.Close()
			RunUIExe()
			outputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgOutput.bin"), "rb")
			structName = (outputFile.ReadString())
			dlgOutput.Read(outputFile)
			outputFile.Close()
			
			if( not (dlgOutput.GetContinueExport())):
				hide_progress_bar()
				return (2)

		except RuntimeError as ex:
			inputFile.Close()
			outputFile.Close()
			show_error_box("Kerrax 3DS Exporter", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
			hide_progress_bar()
			return (2)

	saver = (New3DSFileSaver())
	try:
		selectedObjects = (dlgOutput.GetSelectedObjects())
		useLocalCS = (dlgOutput.GetUseLocalCS())
		spaceTransform = (dlgOutput.GetSpaceTransform())
		matRenParamsPath = (dlgOutput.GetMatRenParamsPath())
		saver.Write3DSFile(EXPORT_FILE_NAME, selectedObjects, useLocalCS, spaceTransform, matRenParamsPath)
	
	except RuntimeError as ex:
		show_error_box("Kerrax 3DS Exporter", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
		hide_progress_bar()
		return (0)

	if( not (QUIET)):
		show_progress_bar("Showing dialog", 98)
		dlgInfo = (NewMeshInfoDlgInput())
		dlgInfo.SetFileName(EXPORT_FILE_NAME)
		dlgInfo.SetFileSize(get_file_size(EXPORT_FILE_NAME))
		dlgInfo.SetExportMode(True)
		dlgInfo.SetObjectStats(saver.GetObjectStats())
		try:
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("MeshInfoDlgInput")
			dlgInfo.Write(inputFile)
			inputFile.Close()
			RunUIExe()
		
		except RuntimeError as ex:
			inputFile.Close()
			show_error_box("Kerrax 3DS Exporter", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))

	hide_progress_bar()
	return (1)


