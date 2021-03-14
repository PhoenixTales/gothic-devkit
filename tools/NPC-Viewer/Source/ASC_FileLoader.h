#include <Irrlicht.h>
using namespace irr;


class ASC_FileLoader
{
private:
	irr::io::IFileSystem* filesystem;
	irr::scene::ISceneManager* smgr;
	irr::IrrlichtDevice* device;

	scene::SMesh* mesh;

	const char* P;
	bool end;
	char* buffer;

	core::array<video::SMaterial> materials;


	irr::s32 readInt();
	irr::f32 readFloat();
	irr::core::stringc readString();

	irr::core::vector3df readVector3df();
	irr::core::vector2df readVector2df();
	irr::video::SColor readColor();	//RGB

	irr::core::stringc getNextToken();

	void gotoNextNoneWhiteSpace();
	void gotoNextNumber();		//digits and '-'
	void gotoNextLine();


	void parseScene();
	void parseDataObjectMaterialList();
	void parseDataObjectMaterial();
	void parseDataObjectSubMaterial(video::SMaterial& mat, u8 count);
	void parseDataObjectGeom();

	void parseMesh(scene::SMeshBuffer* meshbuffer);

	void parseUnknownDataObject();


public:
	__declspec(deprecated("Do not use ASC_FileLoader! It does not work!")) ASC_FileLoader(irr::IrrlichtDevice* device);


	irr::scene::IAnimatedMesh* loadFile(irr::io::path filename);
	irr::scene::IAnimatedMesh* loadFile(irr::io::IReadFile* file);
};
