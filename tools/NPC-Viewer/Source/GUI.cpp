#include "GUI.h"
#include "EventReceiver.h"

namespace edit
{
	GUI::GUI(EventReceiver* evt, gui::IGUIEnvironment* env, List& hml, List& htl, List& btl, irr::io::IAttributes* attr)
		: eventreceiver(evt), guienv(env), headmeshlist(hml), headtexlist(htl), bodytexlist(btl), gender((GENDER)-1), skincolor((SKINCOLOR)-1), attrib(attr)
	{
		guienv->grab();
		
		guienv->getSkin()->setFont(guienv->getFont(attrib->getAttributeAsString("Font")));


		guienv->getSkin()->setColor(gui::EGDC_3D_DARK_SHADOW, attrib->getAttributeAsColor("EGDC_3D_DARK_SHADOW"));
		guienv->getSkin()->setColor(gui::EGDC_3D_SHADOW, attrib->getAttributeAsColor("EGDC_3D_SHADOW"));
		guienv->getSkin()->setColor(gui::EGDC_3D_FACE, attrib->getAttributeAsColor("EGDC_3D_FACE"));
		guienv->getSkin()->setColor(gui::EGDC_3D_HIGH_LIGHT, attrib->getAttributeAsColor("EGDC_3D_HIGH_LIGHT"));
		guienv->getSkin()->setColor(gui::EGDC_3D_LIGHT, attrib->getAttributeAsColor("EGDC_3D_LIGHT"));
		guienv->getSkin()->setColor(gui::EGDC_ACTIVE_BORDER, attrib->getAttributeAsColor("EGDC_ACTIVE_BORDER"));
		guienv->getSkin()->setColor(gui::EGDC_ACTIVE_CAPTION, attrib->getAttributeAsColor("EGDC_ACTIVE_CAPTION"));
		guienv->getSkin()->setColor(gui::EGDC_APP_WORKSPACE, attrib->getAttributeAsColor("EGDC_APP_WORKSPACE"));
		guienv->getSkin()->setColor(gui::EGDC_BUTTON_TEXT, attrib->getAttributeAsColor("EGDC_BUTTON_TEXT"));
		guienv->getSkin()->setColor(gui::EGDC_GRAY_TEXT, attrib->getAttributeAsColor("EGDC_GRAY_TEXT"));
		guienv->getSkin()->setColor(gui::EGDC_HIGH_LIGHT, attrib->getAttributeAsColor("EGDC_HIGH_LIGHT"));
		guienv->getSkin()->setColor(gui::EGDC_HIGH_LIGHT_TEXT, attrib->getAttributeAsColor("EGDC_HIGH_LIGHT_TEXT"));
		guienv->getSkin()->setColor(gui::EGDC_INACTIVE_BORDER, attrib->getAttributeAsColor("EGDC_INACTIVE_BORDER"));
		guienv->getSkin()->setColor(gui::EGDC_INACTIVE_CAPTION, attrib->getAttributeAsColor("EGDC_ACTIVE_CAPTION"));
		guienv->getSkin()->setColor(gui::EGDC_TOOLTIP, attrib->getAttributeAsColor("EGDC_TOOLTIP"));
		guienv->getSkin()->setColor(gui::EGDC_TOOLTIP_BACKGROUND, attrib->getAttributeAsColor("EGDC_TOOLTIP_BACKGROUND"));
		guienv->getSkin()->setColor(gui::EGDC_SCROLLBAR, attrib->getAttributeAsColor("EGDC_SCROLLBAR"));
		guienv->getSkin()->setColor(gui::EGDC_WINDOW, attrib->getAttributeAsColor("EGDC_WINDOW"));
		guienv->getSkin()->setColor(gui::EGDC_ICON, attrib->getAttributeAsColor("EGDC_ICON"));
		guienv->getSkin()->setColor(gui::EGDC_ICON_HIGH_LIGHT, attrib->getAttributeAsColor("EGDC_ICON_HIGH_LIGHT"));



		guienv->addStaticText(L"Gender: ", core::recti(5,5,60,20));
		ctrl_gender = guienv->addComboBox(core::recti(80,2,200,28), 0, EGN_GENDER);
		ctrl_gender->addItem(L"Male", 0);
		ctrl_gender->addItem(L"Female", 1);

		guienv->addStaticText(L"Skincolor: ", core::recti(5,35,80,60));
		ctrl_skincolor = guienv->addComboBox(core::recti(80,32,200,58), 0, EGN_SKINCOLOR);
		for (u8 i=0; i<=SKINCOLOR_ALL; ++i)
			ctrl_skincolor->addItem(SKINCOLORNAMES[i], i);
		ctrl_skincolor->setSelected(SKINCOLOR_NORMAL);

		guienv->addStaticText(L"Headmesh: ", core::recti(5,100,90,130));
		ctrl_headmesh = guienv->addComboBox(core::recti(90,100,250,125),0, EGN_HEADMESH);

		guienv->addStaticText(L"Bodytexture: ", core::recti(5,130,90,160));
		ctrl_bodytexture = guienv->addComboBox(core::recti(90,130,250,155),0, EGN_BODYTEXTURE);

		guienv->addStaticText(L"Headmesh: ", core::recti(5,160,90,190));
		ctrl_headtexture = guienv->addComboBox(core::recti(90,160,250,185),0, EGN_HEADTEXTURE);


		u32 width = guienv->getVideoDriver()->getScreenSize().Width;

		gui::IGUIWindow *rightwindow = guienv->addWindow(core::recti(width-200,0,width,guienv->getVideoDriver()->getScreenSize().Height), false);
		rightwindow->setDraggable(false);
		rightwindow->setDrawTitlebar(false);
		rightwindow->setDrawBackground(false);
		rightwindow->getCloseButton()->setVisible(false);
		guienv->addStaticText(L"ModelScale: ", core::recti(80,5,180,20), false, true, rightwindow);

		ctrl_scale_x_text = guienv->addStaticText(L"X: 1.00", core::recti(0,25,50,40), false, true, rightwindow);
		ctrl_scale_x = guienv->addScrollBar(true, core::recti(50,25,195,40), rightwindow, EGN_SCALE_X);
		ctrl_scale_x->setMin(0);
		ctrl_scale_x->setMax(20);
		ctrl_scale_x->setPos(10);
		ctrl_scale_x->setSmallStep(1);

		ctrl_scale_y_text = guienv->addStaticText(L"Y: 1.00", core::recti(0,45,50,60), false, true, rightwindow);
		ctrl_scale_y = guienv->addScrollBar(true, core::recti(50,45,195,60), rightwindow, EGN_SCALE_Y);
		ctrl_scale_y->setMin(0);
		ctrl_scale_y->setMax(20);
		ctrl_scale_y->setPos(10);
		ctrl_scale_y->setSmallStep(1);

		ctrl_scale_z_text = guienv->addStaticText(L"Z: 1.00", core::recti(0,65,50,80), false, true, rightwindow);
		ctrl_scale_z = guienv->addScrollBar(true, core::recti(50,65,195,80), rightwindow, EGN_SCALE_Z);
		ctrl_scale_z->setMin(0);
		ctrl_scale_z->setMax(20);
		ctrl_scale_z->setPos(10);
		ctrl_scale_z->setSmallStep(1);

		Update();
	}
	GUI::~GUI()
	{
		guienv->drop();
	}
	bool GUI::OnEvent(const SEvent::SGUIEvent &event)
	{
		s32 id = event.Caller->getID();
		switch (event.EventType)
		{
		case gui::EGET_COMBO_BOX_CHANGED:
			if (id==EGN_GENDER || id==EGN_SKINCOLOR)
				Update();
			else if (id==EGN_HEADMESH || id==EGN_BODYTEXTURE || id==EGN_HEADTEXTURE)
			{
				GENDER gender = static_cast<GENDER>(ctrl_gender->getSelected());
				core::stringw headmesh = ctrl_headmesh->getItem(ctrl_headmesh->getSelected());
				core::stringw headtex = ctrl_headtexture->getItem(ctrl_headtexture->getSelected());
				core::stringw bodytex = ctrl_bodytexture->getItem(ctrl_bodytexture->getSelected());
				eventreceiver->Set(gender, headmesh, headtex, bodytex);
			}
			break;
		case gui::EGET_SCROLL_BAR_CHANGED:
			if (id==EGN_SCALE_X)
			{
				core::stringw text = "X: ";
				text.append(core::stringw(f32(ctrl_scale_x->getPos())/10), 4);
				ctrl_scale_x_text->setText(text.c_str());
			}
			else if (id==EGN_SCALE_Y)
			{
				core::stringw text = "Y: ";
				text.append(core::stringw(f32(ctrl_scale_y->getPos())/10), 4);
				ctrl_scale_y_text->setText(text.c_str());
			}
			else if (id==EGN_SCALE_Z)
			{
				core::stringw text = "Z: ";
				text.append(core::stringw(f32(ctrl_scale_z->getPos())/10), 4);
				ctrl_scale_z_text->setText(text.c_str());
			}
			else break;	//Break if not scale-bar

			{
				core::vector3df scale;
				scale.X = f32(ctrl_scale_x->getPos())/10;
				scale.Y = f32(ctrl_scale_y->getPos())/10;
				scale.Z = f32(ctrl_scale_z->getPos())/10;
	
				eventreceiver->SetScale(scale);
			}
		}
		return false;
	}
	void GUI::Update()
	{
		GENDER g = static_cast<GENDER>(ctrl_gender->getSelected());
		SKINCOLOR sk = static_cast<SKINCOLOR>(ctrl_skincolor->getSelected());

		if (g==gender && sk==skincolor)
			return;

		bool sameGender = (g==gender);

		gender = g;
		skincolor = sk;

		core::array<ListItem> list;
		
		core::stringw headmesh;
		core::stringw headtex;
		core::stringw bodytex;

		if (!sameGender)
		{
			ctrl_headmesh->clear();		//Remove all items
			list = headmeshlist.GetItemsFiltered(gender);
			for (u32 i=0; i<list.size(); ++i)
				ctrl_headmesh->addItem(list[i].name.c_str(), i);

			if (list.size())
				headmesh = list[0].name;	//Get first
		}
		else
			headmesh = ctrl_headmesh->getItem(ctrl_headmesh->getSelected());	//get selected


		ctrl_headtexture->clear();		//Remove all items
		list = headtexlist.GetItemsFiltered(gender, skincolor);
		for (u32 i=0; i<list.size(); ++i)
			ctrl_headtexture->addItem(list[i].name.c_str(), i);

		if (list.size())
			headtex = list[0].name;	//Get first

		ctrl_bodytexture->clear();		//Remove all items
		list = bodytexlist.GetItemsFiltered(gender, skincolor);
		for (u32 i=0; i<list.size(); ++i)
			ctrl_bodytexture->addItem(list[i].name.c_str(), i);

		if (list.size())
			bodytex = list[0].name;	//Get first


		eventreceiver->Set(gender, headmesh, headtex, bodytex);
	}
}