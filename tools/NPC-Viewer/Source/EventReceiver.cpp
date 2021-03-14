#include "EventReceiver.h"
#include "Pawn.h"
#include "Constants.h"
#include <Windows.h>

namespace edit
{
	const f32 movespeed = 1.0f;		//Einheiten pro sekunde
	const f32 turnspeed = 45;	//45° pro Sekunde
	const f32 TAU		= core::PI*2;

	const char* LOGLEVEL_TEXT[] = {
		"Info",
		"Warning",
		"Error",
		0
	};

#define SQ(x) ((x)*(x))

	EventReceiver::EventReceiver(irr::IrrlichtDevice* D, Pawn* P, irr::io::IAttributes* attr)
		: device(D), pawn(P), Gui(0), attrib(attr), logfile(0)
	{
		device->setEventReceiver(this);

		logfile = device->getFileSystem()->createAndWriteFile("Lastlog.log");

		for (u32 i=0; i<irr::KEY_KEY_CODES_COUNT; ++i)
			keys[i] = false;

		LoadLists();

		Gui = new GUI(this, device->getGUIEnvironment(), headmeshlist, headtexlist, bodytexlist, attrib);

		camera = device->getSceneManager()->getActiveCamera();
	}
	EventReceiver::~EventReceiver()
	{
		delete Gui;
		device->setEventReceiver(0);
	}
	bool EventReceiver::OnEvent(const SEvent& event)
	{
		if (event.EventType == EET_KEY_INPUT_EVENT)
		{
			keys[event.KeyInput.Key] = event.KeyInput.PressedDown;
			if (keys[irr::KEY_UP] || keys[irr::KEY_DOWN] || keys[irr::KEY_LEFT] || keys[irr::KEY_RIGHT]
			|| keys[irr::KEY_NUMPAD8] || keys[KEY_NUMPAD2] || keys[KEY_NUMPAD4] || keys[KEY_NUMPAD6]
			|| keys[irr::KEY_NEXT] || keys[irr::KEY_PRIOR])
				return true;
		}
		if (event.EventType == EET_GUI_EVENT)
		{
			return Gui->OnEvent(event.GUIEvent);
		}
		if (event.EventType == EET_LOG_TEXT_EVENT)
		{
			logfile->write(LOGLEVEL_TEXT[event.LogEvent.Level], strlen(LOGLEVEL_TEXT[event.LogEvent.Level]));
			logfile->write(": ", 2);
			logfile->write(event.LogEvent.Text, strlen(event.LogEvent.Text));

			if (event.LogEvent.Level == ELL_ERROR)
			{
				logfile->write("Exit process", strlen("Exit process"));
				ExitProcess(666);
				return true;
			}
			logfile->write("\r\n", 2);
		}
		return false;
	}
	SKINCOLOR EventReceiver::TextToSkincolor(const char* skincolor) const
	{
		core::stringc color = skincolor;
		color.make_upper();

		if (color=="PALE")
			return SKINCOLOR_PALE;
		if (color=="LATINO")
			return SKINCOLOR_LATINO;
		if (color=="BROWN")
			return SKINCOLOR_BROWN;
		if (color=="EXTRA")
			return SKINCOLOR_EXTRA;

		return SKINCOLOR_NORMAL;
	}
	GENDER EventReceiver::TextToGender(const char* gender) const
	{
		core::stringc g = gender;
		g.make_upper();

		if (g=="FEMALE")
			return FEMALE;
		return MALE;
	}
	void EventReceiver::LoadLists()
	{
		io::IXMLReaderUTF8* xml = device->getFileSystem()->createXMLReaderUTF8(L"Classification.xml");
		while (xml->read())
		{
			if (core::stringw(xml->getNodeName())=="headmesh")
			{
				ListItem item;
				item.name = xml->getAttributeValueSafe("name");
				item.gender = TextToGender(xml->getAttributeValueSafe("gender"));

				if (item.name!=0 && item.name!="")
					headmeshlist.Add(item);
			}
			else if (core::stringw(xml->getNodeName())=="headtex")
			{
				ListItem item;
				item.name = xml->getAttributeValueSafe("name");
				item.gender = TextToGender(xml->getAttributeValueSafe("gender"));
				item.id = xml->getAttributeValueAsInt("id");
				item.skincolor = TextToSkincolor(xml->getAttributeValueSafe("skincolor"));

				if (item.name!=0 && item.name!="")
					headtexlist.Add(item);
			}
			else if (core::stringw(xml->getNodeName())=="bodytex")
			{
				ListItem item;
				item.name = xml->getAttributeValueSafe("name");
				item.gender = TextToGender(xml->getAttributeValueSafe("gender"));
				item.id = xml->getAttributeValueAsInt("id");
				item.skincolor = TextToSkincolor(xml->getAttributeValueSafe("skincolor"));

				if (item.name!=0 && item.name!="")
					bodytexlist.Add(item);
			}
		}
	}
	void EventReceiver::Set(GENDER g, const core::stringw& headmeshname, const core::stringw& headtexname, const core::stringw& bodytexname)
	{
		if (g!=pawn->GetGender())
			pawn->SetGender(g);

		pawn->SetHeadMesh(headmeshname);
		ListItem *li = headtexlist.GetItem(headtexname);
		if (!li)
			pawn->SetHeadTexture(-1);
		else
			pawn->SetHeadTexture(li->id);
		
		li = bodytexlist.GetItem(bodytexname);
		if (!li)
			pawn->SetBodyTexture(-1);
		else
			pawn->SetBodyTexture(li->id);
	}
	void EventReceiver::SetScale(const core::vector3df& scale)
	{
		pawn->SetModelScale(scale);
	}

	void EventReceiver::OnRender()
	{
		u32 timedif = device->getTimer()->getRealTime()-lasttime;
		lasttime = device->getTimer()->getRealTime();

		if (keys[irr::KEY_UP] || keys[irr::KEY_DOWN] || keys[irr::KEY_LEFT] || keys[irr::KEY_RIGHT]
		|| keys[irr::KEY_NUMPAD8] || keys[KEY_NUMPAD2] || keys[KEY_NUMPAD4] || keys[KEY_NUMPAD6]
		|| keys[irr::KEY_NEXT] || keys[irr::KEY_PRIOR])
		{
			core::vector3df pos = camera->getPosition();
			core::vector3df target = camera->getTarget();

			if (keys[KEY_UP])		//nach vorne
			{
				f32 X = -sinf(rotation.X)*movespeed*timedif/1000;
				f32 Z = cosf(rotation.X)*movespeed*timedif/1000;

				pos.X += X;
				pos.Z += Z;
				target.X += X;
				target.Z += Z;
			}
			if (keys[KEY_DOWN])	//nach hinten
			{
				f32 X = sinf(rotation.X)*movespeed*timedif/1000;
				f32 Z = -cosf(rotation.X)*movespeed*timedif/1000;

				pos.X += X;
				pos.Z += Z;
				target.X += X;
				target.Z += Z;
			}
			if (keys[KEY_LEFT])		//nach links
			{
				f32 X = -cosf(rotation.X)*movespeed*timedif/1000;
				f32 Z = -sinf(rotation.X)*movespeed*timedif/1000;

				pos.X += X;
				pos.Z += Z;
				target.X += X;
				target.Z += Z;
			}
			if (keys[KEY_RIGHT])	//nach rechts
			{
				f32 X = cosf(rotation.X)*movespeed*timedif/1000;
				f32 Z = sinf(rotation.X)*movespeed*timedif/1000;

				pos.X += X;
				pos.Z += Z;
				target.X += X;
				target.Z += Z;
			}

			if (keys[KEY_NUMPAD6])
			{
				f32 change = (turnspeed*timedif/1000);
				rotation.X += (change*core::DEGTORAD);
				pos.rotateXZBy(change);
			}
			if (keys[KEY_NUMPAD4])
			{
				f32 change = -(turnspeed*timedif/1000);
				rotation.X += (change*core::DEGTORAD);
				pos.rotateXZBy(change);
			}
			if (keys[KEY_NUMPAD8])
			{
				target.Y += turnspeed*timedif/10000;
			}
			if (keys[KEY_NUMPAD2])
			{
				target.Y -= turnspeed*timedif/10000;
			}
			if (keys[KEY_PRIOR])
			{
				pos.Y += movespeed*timedif/1000;
				target.Y += movespeed*timedif/1000;
			}
			if (keys[KEY_NEXT])
			{
				pos.Y -= movespeed*timedif/1000;
				target.Y -= movespeed*timedif/1000;
			}

			if (rotation.X>=TAU)
				rotation.X -= TAU;
			else if (rotation.X<0.f)
				rotation.X += TAU;
			
			/*if (rotation.Y>1.5f)
				rotation.Y = 1.5f;
			else if (rotation.Y<-1.5f)
				rotation.Y = -1.5f;*/


			camera->setPosition(pos);
			camera->setTarget(target);
		}
	}
}