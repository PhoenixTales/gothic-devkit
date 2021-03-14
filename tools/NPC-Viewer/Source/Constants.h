#ifndef __CONSTANTS_H__
#define __CONSTANTS_H__
#include <Irrlicht.h>

namespace edit
{
	using namespace irr;

	static const char* body_male	= "HUM_BODY_NAKED0.x";
	static const char* body_female	= "HUM_BODY_BABE0.x";
	static const char* defaulthead	= "HUM_HEAD_BALD.x";
	static const char* defaulthead_noext = "HUM_HEAD_BALD";	//ohne Dateiendung
	static const wchar_t* defaulthead_noext_w = L"HUM_HEAD_BALD";	//ohne Dateiendung

	enum GENDER
	{
		MALE,
		FEMALE
	};

	enum SKINCOLOR
	{
		SKINCOLOR_PALE,
		SKINCOLOR_NORMAL,
		SKINCOLOR_LATINO,
		SKINCOLOR_BROWN,
		SKINCOLOR_EXTRA,	//Für Spielertextur
		SKINCOLOR_ALL	//Für Spielertextur
	};
	static const wchar_t* SKINCOLORNAMES[] = {
		L"Pale",
		L"Normal",
		L"Latino",
		L"Brown",
		L"Extra",
		L"ALL"
	};
};
#endif