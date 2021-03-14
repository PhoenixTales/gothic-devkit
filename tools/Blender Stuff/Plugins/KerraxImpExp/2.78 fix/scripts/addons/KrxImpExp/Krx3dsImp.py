
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

class T3DSReadChunk:
	def Init(self):
		self. __chunkPos = (0)
		self. __chunkID = (0)
		self. __chunkSize = (0)
		self. __dataSize = (0)

	def ReadHeader(self, file):
		self. __chunkPos = (file.GetPos())
		self. __chunkID = (file.ReadUnsignedShort())
		self. __chunkSize = (file.ReadUnsignedLong())
		self. __dataSize = (0)

	def SkipRest(self, file):
		newPos = (self. __chunkPos + self. __chunkSize)
		file.SetPos(newPos)

	def GetID(self):
		return (self. __chunkID)

	def GetPos(self):
		return (self. __chunkPos)

	def GetSize(self):
		return (self. __chunkSize)

	def SetDataSize(self, dataSize):
		self. __dataSize = (dataSize)	

	def GetDataSize(self):
		return (self. __dataSize)

	def GetSubChunksSize(self):
		return (self. __chunkSize - self. __dataSize - 6)

def New3DSReadChunk():
	chunk = (T3DSReadChunk())
	chunk.Init()
	return (chunk)

def DelObjects(objects):
	i = ((len(objects)) - 1)
	while(i >= 0):
		obj = (((objects)[i]))
		delete_object(obj)
		i = (i - 1)

class T3DSFileLoader:
	def __Read3DSVersion(self, parentChunk):
		self. __3DSVersion = (self. __file.ReadUnsignedLong())

	def __ReadMeshVersion(self, parentChunk):
		self. __meshVersion = (self. __file.ReadUnsignedLong())

	def __ReadColor(self, parentChunk):
		r = (0)
		g = (0)
		b = (0)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if((chunk.GetID() == 0x0010) or (chunk.GetID() == 0x0011)):
				if(chunk.GetID() == 0x0010):
					r = (self. __file.ReadFloat())
					g = (self. __file.ReadFloat())
					b = (self. __file.ReadFloat())
				else:
					r = ((int_to_float(self. __file.ReadUnsignedChar())) / 255)
					g = ((int_to_float(self. __file.ReadUnsignedChar())) / 255)
					b = ((int_to_float(self. __file.ReadUnsignedChar())) / 255)
				
				if(r < 0):
					r = (0)
				elif(r > 1):
					r = (1)
				
				if(g < 0):
					g = (0)
				elif(g > 1):
					g = (1)
				
				if(b < 0):
					b = (0)
				elif(b > 1):
					b = (1)

			chunk.SkipRest(self. __file)
		
		clr = (new_color(r, g, b))
		return (clr)

	def __ReadPercent(self, parentChunk):
		p = (0)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if((chunk.GetID() == 0x0030) or (chunk.GetID() == 0x0031)):
				if(chunk.GetID() == 0x0030):
					p = (self. __file.ReadUnsignedShort())
				else:
					p = ((float_to_int(self. __file.ReadFloat())))
				
				if(p < 0):
					p = (0)
				elif(p > 100):
					p = (100)

			chunk.SkipRest(self. __file)
		
		return (p)

	def __ReadMap(self, parentChunk):
		mapname = ("")
		mapnameRead = (False)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0 and not (mapnameRead)):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0xA300):
				mapname = (self. __file.ReadString())
				mapnameRead = (True)
			
			chunk.SkipRest(self. __file)
		
		return (mapname)

	def __ReadMaterialBlock(self, parentChunk):
		numMtls = ((len(self. __impMtls)))
		mtl = (None)
		mtlName = ("")
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0xA000):
				mtlName = (self. __file.ReadString())
				caption = ("Reading materials")
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
				mtl = (new_material(mtlName))
				self. __impMtls.append(mtl)
				self. __impMtlNames.append(mtlName)
			elif(chunk.GetID() == 0xA020):
				set_diffuse_color(mtl, self. __ReadColor(chunk))
			elif(chunk.GetID() == 0xA200):
				set_diffuse_map_filename(mtl, self. __ReadMap(chunk))
			elif(chunk.GetID() == 0xA010):
				ambColor = (self. __ReadColor(chunk))
			elif(chunk.GetID() == 0xA030):
				specColor = (self. __ReadColor(chunk))
			elif(chunk.GetID() == 0xA040):
				shininessPercent = (self. __ReadPercent(chunk))
			elif(chunk.GetID() == 0xA041):
				shininessStrengthPercent = (self. __ReadPercent(chunk))
			elif(chunk.GetID() == 0xA050):
				transparencyPercent = (self. __ReadPercent(chunk))
			elif(chunk.GetID() == 0xA052):
				transparencyFalloffPercent = (self. __ReadPercent(chunk))
			elif(chunk.GetID() == 0xA053):
				reflectionBlurPercent = (self. __ReadPercent(chunk))
			
			chunk.SkipRest(self. __file)

	def __ReadOneUnit(self, parentChunk):
		self. __oneUnit = (self. __file.ReadFloat())

	def __ReadVerticesList(self, parentChunk):
		numVerts = (self. __file.ReadUnsignedShort())
		
		self. __curObjMesh.set_num_verts(numVerts)
		caption = (FormatMsg1("Reading '%1'", self. __curObjNameInFile))
		for i in range((0), (numVerts)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			x = (self. __file.ReadFloat() * self. __scaleCoef)
			y = (self. __file.ReadFloat() * self. __scaleCoef)
			z = (self. __file.ReadFloat() * self. __scaleCoef)
			pt = (new_vector(x, y, z))
			(self. __curObjMesh.set_vert(i, pt))

	def __CreateTVFaces(self):
		if((self. __curObjMesh.get_num_tverts()) == 0):
			return
		
		numFaces = ((self. __curObjMesh.get_num_faces()))
		self. __curObjMesh.set_num_tvfaces(self. __curObjMesh.get_num_faces())
		for i in range((0), (numFaces)):
			face = ((self. __curObjMesh.get_face(i)))
			v0 = (get_face_vert(face, 0))
			v1 = (get_face_vert(face, 1))
			v2 = (get_face_vert(face, 2))
			tvface = (new_tvface(v0, v1, v2))
			self. __curObjMesh.set_tvface(i, tvface)

	def __FindMaterial(self, mtlName):
		for i in range((0), ((len(self. __impMtlNames)))):
			if(stricmp(((self. __impMtlNames)[i]), mtlName) == 0):
				return (((self. __impMtls)[i]))

		raise RuntimeError(FormatMsg2("Material not found.\nMaterial name: \"%1\".\nFile name: \"%2\".", mtlName, self. __file.GetName()))
		return (None)

	def __ReadSmoothGroupList(self, parentChunk):
		numFaces = ((self. __curObjMesh.get_num_faces()))
		for i in range((0), (numFaces)):
			smGroup = (self. __file.ReadUnsignedLong())
			pass

	def __ReadFacesMtlList(self, parentChunk):
		mtlName = (self. __file.ReadString())
		mtl = (self. __FindMaterial(mtlName))
		numEntries = (self. __file.ReadUnsignedShort())
		numFaces = ((self. __curObjMesh.get_num_faces()))
		for i in range((0), (numEntries)):
			faceIndex = (self. __file.ReadUnsignedShort())
			if(faceIndex >= numFaces):
				raise RuntimeError(FormatMsg4("Face index is out of range while reading faces material list.\nFace index: %1 (Allowable range: %2..%3).\nFile name: \"%4\".", (int_to_string(faceIndex)), "0", (int_to_string(numFaces - 1)), self. __file.GetName()))
			
			self. __curObjMesh.set_face_material(faceIndex, mtl)

	def __ReadFacesDescription(self, parentChunk):
		numFaces = (self. __file.ReadUnsignedShort())
		
		self. __curObjMesh.set_num_faces(numFaces)
		caption = (FormatMsg1("Reading '%1'", self. __curObjNameInFile))
		numVerts = ((self. __curObjMesh.get_num_verts()))
		for i in range((0), (numFaces)):
			if(((i) & (31)) == 0):
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			v0 = (self. __file.ReadUnsignedShort())
			v1 = (self. __file.ReadUnsignedShort())
			v2 = (self. __file.ReadUnsignedShort())
			flags = (self. __file.ReadUnsignedShort())
			if((v0 >= numVerts) or (v1 >= numVerts) or (v2 >= numVerts)):
				verr = (v2)
				if(v0 >= numVerts):
					verr = (v0)
				elif(v1 >= numVerts):
					verr = (v1)
				
				raise RuntimeError(FormatMsg4("Vertex index is out of range while reading faces description.\nVertex index: %1 (Allowable range: %2..%3).\nFile name: \"%4\".", (int_to_string(verr)), "0", (int_to_string(numVerts - 1)), self. __file.GetName()))
			
			face = (new_face(v0, v1, v2))
			self. __curObjMesh.set_face(i, face)
			visCA = ((((flags) & (0x01)) != 0))
			visBC = ((((flags) & (0x02)) != 0))
			visAB = ((((flags) & (0x04)) != 0))
			pass
			pass
			pass
		
		self. __CreateTVFaces()
		
		parentChunk.SetDataSize(2 + 8 * numFaces)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0x4130):
				self. __ReadFacesMtlList(chunk)
			elif(chunk.GetID() == 0x4150):
				self. __ReadSmoothGroupList(chunk)
			
			chunk.SkipRest(self. __file)

	def __ReadMappingCoords(self, parentChunk):
		numTVerts = (self. __file.ReadUnsignedShort())
		
		self. __curObjMesh.set_num_tverts(numTVerts)
		caption = (FormatMsg1("Reading '%1'", self. __curObjNameInFile))
		for i in range((0), (numTVerts)):
			if(((i) & (63)) == 0):
				show_progress_bar(caption, 100 * self. __file.GetPos() / self. __file.GetSize())
			
			u = (self. __file.ReadFloat())
			v = (self. __file.ReadFloat())
			uvvert = (new_uvvert(u, v))
			self. __curObjMesh.set_tvert(i, uvvert)

	def __EndReadingMesh(self):
		objects = ((self. __curObjMesh.update()))
		for i in range((0), ((len(objects)))):
			obj = (((objects)[i]))
			objStats = (NewObjectStats())
			objStats.SetNameInFile(self. __curObjNameInFile)
			objStats.SetNameInScene((get_object_name(obj)))
			objStats.SetNumMtls((calculate_num_materials(obj)))
			objStats.SetNumFaces((calculate_num_faces(obj)))
			objStats.SetNumVertsInFile((calculate_num_verts(obj)))
			objStats.SetNumVertsInScene(objStats.GetNumVertsInFile())
			self. __impObjectStats.append(objStats)
			if(i != 0):
				self. __impObjects.append(obj)

	def __ReadMesh(self, parentChunk):
		self. __curObj = (new_mesh_object(self. __curObjNameInScene))
		set_transform(self. __curObj, identity_matrix())
		self. __curObjMesh = (MeshData(self. __curObj))
		self. __impObjects.append(self. __curObj)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0x4110):
				self. __ReadVerticesList(chunk)
			elif(chunk.GetID() == 0x4140):
				self. __ReadMappingCoords(chunk)
			elif(chunk.GetID() == 0x4120):
				self. __ReadFacesDescription(chunk)
			
			chunk.SkipRest(self. __file)
		
		self. __EndReadingMesh()

	def __ReadObjectHidden(self, parentChunk):
		self. __curObjHidden = (True)

	def __ReadObjectBlock(self, parentChunk):
		self. __curObjNameInFile = (self. __file.ReadString())
		parentChunk.SetDataSize(len(self. __curObjNameInFile) + 1)
		if((find_object_by_name(self. __curObjNameInFile)) == None):
			self. __curObjNameInScene = (self. __curObjNameInFile)
		else:
			self. __curObjNameInScene = ((unique_object_name(self. __curObjNameInFile)))
		
		self. __curObjHidden = (False)
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0x4010):
				self. __ReadObjectHidden(chunk)
			elif(chunk.GetID() == 0x4100):
				self. __ReadMesh(chunk)
			
			chunk.SkipRest(self. __file)
		
		if(self. __curObjHidden):
			show(self. __curObj, False)

	def __Read3DEditor(self, parentChunk):
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0x3D3E):
				self. __ReadMeshVersion(chunk)
			elif(chunk.GetID() == 0xAFFF):
				self. __ReadMaterialBlock(chunk)
			elif(chunk.GetID() == 0x0100):
				self. __ReadOneUnit(chunk)
			elif(chunk.GetID() == 0x4000):
				self. __ReadObjectBlock(chunk)
			
			chunk.SkipRest(self. __file)

	def __ReadMainChunk(self, parentChunk):
		chunk = (New3DSReadChunk())
		sizeOfChunks = (parentChunk.GetSubChunksSize())
		while(sizeOfChunks > 0):
			chunk.ReadHeader(self. __file)
			sizeOfChunks = (sizeOfChunks - chunk.GetSize())
			if(chunk.GetID() == 0x0002):
				self. __Read3DSVersion(chunk)
			elif(chunk.GetID() == 0x3D3D):
				self. __Read3DEditor(chunk)
			
			chunk.SkipRest(self. __file)

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

	def Init(self):
		self. __scaleCoef = (1.0)
		self. __file = (NewFile())
		self. __3DSVersion = (3)
		self. __meshVersion = (3)
		self. __oneUnit = (1.0)
		self. __impMtls = ([])
		self. __impMtlNames = ([])
		self. __impObjects = ([])
		self. __curObj = (None)
		self. __curObjHidden = (False)
		self. __curObjNameInFile = ("")
		self. __curObjMesh = (None)
		self. __impObjectStats = ([])

	def Read3DSFile(self, filename, spaceTransform):
		self. Init()
		self. __SetSpaceTransform(spaceTransform)
		try:
			self. __file.Open(filename, "rb")
			chunk = (New3DSReadChunk())
			chunk.ReadHeader(self. __file)
			if(chunk.GetID() == 0x4D4D):
				self. __ReadMainChunk(chunk)
			else:
				raise RuntimeError(FormatMsg1("File is not a 3d studio mesh.\nFile name: \"%1\".", self. __file.GetName()))
			
			self. __file.Close()
		
		except RuntimeError as ex:
			self. __file.Close()
			
			raise

	def WeldVertices(self, threshold):
		try:
			for i in range((0), ((len(self. __impObjects)))):
				obj = (((self. __impObjects)[i]))
				objStats = (((self. __impObjectStats)[i]))
				objname = (objStats.GetNameInFile())
				caption = (FormatMsg1("Welding vertices", objname))
				show_progress_bar(caption, 100 * i / (len(self. __impObjects)))
				numVertsInScene = (weld_vertices(obj, threshold * self. __scaleCoef))
				objStats.SetNumVertsInScene(numVertsInScene)
				self. __impObjectStats[i] = (objStats)

		except RuntimeError as ex:
			raise

	def GetObjectStats(self):
		return (self. __impObjectStats)

	def ReplaceObjectWithLoaded(self, objname):
		if((len(self. __impObjects)) == 0):
			return
		
		try:
			obj2 = ((find_object_by_name(objname)))
			tm = ((get_transform(obj2)))
			
			obj = (((self. __impObjects)[0]))
			replace_object(obj2, obj)
			
			delete_object(obj)
			self. __impObjects[0] = (obj2)
			
			objst = (((self. __impObjectStats)[0]))
			objst.SetNameInScene(objname)
			self. __impObjectStats[0] = (objst)
			
			for j in range((1), ((len(self. __impObjects)))):
				obj = (((self. __impObjects)[j]))
				set_parent(obj, obj2)
				set_transform(obj, tm)

		except RuntimeError as ex:
			raise

def New3DSFileLoader():
	loader = (T3DSFileLoader())
	loader.Init()
	return (loader)

class T3DSImporterDlgInput:
	def Init(self):
		self. __importFileName = ("")
		self. __importFileSize = (0)
		self. __slots = ([])
		self. __bones = ([])
		self. __selectedSlot = ("")
		self. __selectedBone = ("")
		self. __curSceneMode = (1)
		self. __systemUnitsPerFileUnit = (1)

	def Write(self, f):
		f.WriteString(self. __importFileName)
		f.WriteUnsignedLong(self. __importFileSize)
		f.WriteUnsignedLong((len(self. __slots)))
		for i in range((0), ((len(self. __slots)))):
			f.WriteString(((self. __slots)[i]))
		
		f.WriteUnsignedLong((len(self. __bones)))
		for j in range((0), ((len(self. __bones)))):
			f.WriteString(((self. __bones)[j]))
		
		f.WriteString(self. __selectedSlot)
		f.WriteString(self. __selectedBone)
		f.WriteUnsignedChar(self. __curSceneMode)
		f.WriteFloat(self. __systemUnitsPerFileUnit)

	def Read(self, f):
		self. __importFileName = (f.ReadString())
		self. __importFileSize = (f.ReadUnsignedLong())
		numSlots = (f.ReadUnsignedLong())
		self. __slots = ([])
		
		for i in range((0), (numSlots)):
			self. __slots.append(f.ReadString())
		
		numBones = (f.ReadUnsignedLong())
		self. __bones = ([])
		
		for j in range((0), (numBones)):
			self. __bones.append(f.ReadString())
		
		self. __selectedSlot = (f.ReadString())
		self. __selectedBone = (f.ReadString())
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __systemUnitsPerFileUnit = (f.ReadFloat())

	def SetImportFileName(self, importFileName):
		self. __importFileName = (importFileName)

	def GetImportFileName(self):
		return (self. __importFileName)

	def SetImportFileSize(self, importFileSize):
		self. __importFileSize = (importFileSize)

	def GetImportFileSize(self):
		return (self. __importFileSize)

	def SetSlots(self, slots):
		self. __slots = (slots)

	def GetSlots(self):
		return (self. __slots)

	def SetBones(self, bones):
		self. __bones = (bones)

	def GetBones(self):
		return (self. __bones)

	def SetSelectedSlot(self, selectedSlot):
		self. __selectedSlot = (selectedSlot)

	def GetSelectedSlot(self):
		return (self. __selectedSlot)

	def SetSelectedBone(self, selectedBone):
		self. __selectedBone = (selectedBone)

	def GetSelectedBone(self):
		return (self. __selectedBone)

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetSystemUnitsPerFileUnit(self, scaleCoef):
		self. __systemUnitsPerFileUnit = (scaleCoef)

	def GetSystemUnitsPerFileUnit(self):
		return (self. __systemUnitsPerFileUnit)

def New3DSImporterDlgInput():
	dlgInput = (T3DSImporterDlgInput())
	dlgInput.Init()
	return (dlgInput)

class T3DSImporterDlgOutput:
	def Init(self, dlgInput):
		self. __curSceneMode = (dlgInput.GetCurrentSceneMode())
		self. __selectedSlot = (dlgInput.GetSelectedSlot())
		self. __selectedBone = (dlgInput.GetSelectedBone())
		self. __weldVertices = (False)
		self. __threshold = ((int_to_float(1)) / 10)
		self. __continueImport = (True)
		self. __spaceTransform = (NewSpaceTransform())
		self. __spaceTransform.SetSystemUnitsPerFileUnit(dlgInput.GetSystemUnitsPerFileUnit())

	def Write(self, f):
		f.WriteUnsignedChar(self. __curSceneMode)
		f.WriteString(self. __selectedSlot)
		f.WriteString(self. __selectedBone)
		f.WriteBool(self. __weldVertices)
		f.WriteFloat(self. __threshold)
		self. __spaceTransform.Write(f)
		f.WriteBool(self. __continueImport)

	def Read(self, f):
		self. __curSceneMode = (f.ReadUnsignedChar())
		self. __selectedSlot = (f.ReadString())
		self. __selectedBone = (f.ReadString())
		self. __weldVertices = (f.ReadBool())
		self. __threshold = (f.ReadFloat())
		self. __spaceTransform.Read(f)
		self. __continueImport = (f.ReadBool())

	def SetCurrentSceneMode(self, curSceneMode):
		self. __curSceneMode = (curSceneMode)

	def GetCurrentSceneMode(self):
		return (self. __curSceneMode)

	def SetSelectedSlot(self, selectedSlot):
		self. __selectedSlot = (selectedSlot)

	def GetSelectedSlot(self):
		return (self. __selectedSlot)

	def SetSelectedBone(self, selectedBone):
		self. __selectedBone = (selectedBone)

	def GetSelectedBone(self):
		return (self. __selectedBone)

	def SetWeldVertices(self, weldVertices):
		self. __weldVertices = (weldVertices)

	def GetWeldVertices(self):
		return (self. __weldVertices)

	def SetThreshold(self, threshold):
		self. __threshold = (threshold)

	def GetThreshold(self):
		return (self. __threshold)

	def SetSpaceTransform(self, spaceTransform):
		self. __spaceTransform = (spaceTransform)

	def GetSpaceTransform(self):
		return (self. __spaceTransform)

	def SetContinueImport(self, continueImport):
		self. __continueImport = (continueImport)

	def GetContinueImport(self):
		return (self. __continueImport)

def New3DSImporterDlgOutput(dlgInput):
	dlgOutput = (T3DSImporterDlgOutput())
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
	register_importer("Krx3dsImp", "3DS", "Kerrax 3D Studio Mesh") 

def unregister(): 
	unregister_importer("Krx3dsImp") 

if __name__ == "main": 
	register() 

def Krx3dsImp(filename_param, quiet_param = False): 
	QUIET = quiet_param 
	IMPORT_FILE_NAME = filename_param 
	IMPORT_FILE_SIZE = get_file_size(IMPORT_FILE_NAME)
	
	sceneAnalyzer = (AnalyzeScene())
	
	dlgInput = (New3DSImporterDlgInput())
	dlgInput.SetImportFileName(IMPORT_FILE_NAME)
	dlgInput.SetImportFileSize(IMPORT_FILE_SIZE)
	dlgInput.SetSystemUnitsPerFileUnit(0.01)
	dlgInput.SetCurrentSceneMode(1)
	
	dlgInput.SetSlots(sceneAnalyzer.GetSceneSlots())
	selectedSlots = (sceneAnalyzer.GetSelectedSlots())
	if((len(selectedSlots)) != 0):
		dlgInput.SetSelectedSlot(((selectedSlots)[0]))
		dlgInput.SetCurrentSceneMode(3)

	dlgInput.SetBones(sceneAnalyzer.GetSceneBones())
	selectedBones = (sceneAnalyzer.GetSelectedBones())
	if((len(selectedBones)) != 0):
		dlgInput.SetSelectedBone(((selectedBones)[0]))
		dlgInput.SetCurrentSceneMode(4)

	dlgOutput = (New3DSImporterDlgOutput(dlgInput))
	inputFile = (NewFile())
	outputFile = (NewFile())
	if( not (QUIET)):
		try:
			show_progress_bar("Showing dialog", 0)
			inputFile.Open((get_plugcfg_dir() + "KrxImpExpDlgInput.bin"), "wb")
			inputFile.WriteString("3DSImporterDlgInput")
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
			show_error_box("Kerrax 3DS Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
			hide_progress_bar()
			return (2)

	loader = (New3DSFileLoader())
	try:
		if(dlgOutput.GetCurrentSceneMode() == 1):
			reset_scene()

		spaceTransform = (dlgOutput.GetSpaceTransform())
		loader.Read3DSFile(IMPORT_FILE_NAME, spaceTransform)
		
		if(dlgOutput.GetWeldVertices()):
			loader.WeldVertices(dlgOutput.GetThreshold())

		if(dlgOutput.GetCurrentSceneMode() == 3):
			loader.ReplaceObjectWithLoaded(dlgOutput.GetSelectedSlot())
		elif(dlgOutput.GetCurrentSceneMode() == 4):
			loader.ReplaceObjectWithLoaded(dlgOutput.GetSelectedBone())

	except RuntimeError as ex:
		show_error_box("Kerrax 3DS Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))
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
			show_error_box("Kerrax 3DS Importer", (ex.args[0] if (len(ex.args) >= 1 and type(ex.args[0]) == str) else str(ex.args)))

	hide_progress_bar()
	return (1)


