#include <irrlicht.h>
#include <WIndows.h>
#include "XML.h"
//#include "ASC_FileLoader.h"
#include "Pawn.h"
#include <iostream>
#include "EventReceiver.h"

#pragma comment(lib, "Irrlicht.lib")
#define PrintWarning(text)	MessageBoxA(NULL, text, "Warning", 64)


irr::IrrlichtDevice* device;
irr::scene::ISceneManager* smgr;
irr::video::IVideoDriver* driver;
irr::scene::IAnimatedMeshSceneNode* body;
irr::scene::IAnimatedMeshSceneNode* head;


//INT WINAPI WinMain( HINSTANCE hInst, HINSTANCE, LPSTR strCmdLine, INT )

INT WINAPI WinMain( HINSTANCE hInst, HINSTANCE, LPSTR strCmdLine, INT )
{
	irr::SIrrlichtCreationParameters config = edit::ReadConfig("IrrlichtDevice.xml");

	if (config.DriverType == irr::video::EDT_NULL)
		PrintWarning("Can not create device with the DriverType NULL!");

	device = irr::createDeviceEx(config);

	if (!device)
		return 0;

	
	irr::io::IXMLReader* xml = device->getFileSystem()->createXMLReader("Config.xml");
	irr::io::IAttributes *attrib = device->getFileSystem()->createEmptyAttributes();
	attrib->read(xml);
	xml->drop();	//drop if not more needed


	//device->getFileSystem()->addZipFileArchive("data.zip");
	device->getFileSystem()->addFileArchive("data.zip");
	driver = device->getVideoDriver();
	smgr = device->getSceneManager();

	irr::scene::ICameraSceneNode* camera=smgr->addCameraSceneNode(0, irr::core::vector3df(0,1.0f,-1.5f),irr::core::vector3df(0,1.0f,0));
	camera->setNearValue(0.1f);
	camera->setPosition(irr::core::vector3df(0,1.0f,-1.5f));
	camera->setTarget(irr::core::vector3df(0,1.0f,0));

	edit::Pawn pawn(device);
	edit::EventReceiver EvtRecv(device, &pawn, attrib);

//	pawn.SetArmor(L"Armor_Lester");

	device->setWindowCaption(L"Marthog's Face Helper");

	HWND hwnd = 0;
	if (config.DriverType == irr::video::EDT_DIRECT3D8)
		hwnd = (HWND)driver->getExposedVideoData().D3D8.HWnd;
	else if (config.DriverType == irr::video::EDT_DIRECT3D9)
		hwnd = (HWND)driver->getExposedVideoData().D3D9.HWnd;
	else if (config.DriverType == irr::video::EDT_OPENGL)
		hwnd = (HWND)driver->getExposedVideoData().OpenGLWin32.HWnd;

	if (hwnd)
		SendMessage(hwnd, WM_SETICON, ICON_BIG, (LPARAM)LoadImage(GetModuleHandle(NULL), MAKEINTRESOURCE(1), IMAGE_ICON, 16, 16, 0));
	
	while (device->run())
	{
		EvtRecv.OnRender();

		driver->beginScene(true, true, irr::video::SColor(255,150,150,150));
		smgr->drawAll();
		device->getGUIEnvironment()->drawAll();
		driver->endScene();
		device->yield();
	}
	attrib->drop();
	//device->drop();

	return 0;
}
