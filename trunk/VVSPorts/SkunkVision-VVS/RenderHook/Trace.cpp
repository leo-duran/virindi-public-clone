// Trace.cpp

#include "stdafx.h"
#include "Trace.h"


LPCTSTR PszTraceLight(const D3DLIGHT9* plight)
	{
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%08X->[%u [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f] %1.2f %1.2f [%1.2f %1.2f %1.2f] %1.2f %1.2f ]"), 
		plight, 
		plight->Type, 
		(double)plight->Diffuse.r, (double)plight->Diffuse.g, (double)plight->Diffuse.b, (double)plight->Diffuse.a, 
		(double)plight->Specular.r, (double)plight->Specular.g, (double)plight->Specular.b, (double)plight->Specular.a, 
		(double)plight->Ambient.r, (double)plight->Ambient.g, (double)plight->Ambient.b, (double)plight->Ambient.a, 
		(double)plight->Position.x, (double)plight->Position.y, (double)plight->Position.z, 
		(double)plight->Direction.x, (double)plight->Direction.y, (double)plight->Direction.z, 
		(double)plight->Range, (double)plight->Falloff, 
		(double)plight->Attenuation0, (double)plight->Attenuation1, (double)plight->Attenuation2, 
		(double)plight->Theta, (double)plight->Phi 
		);
	return szBuf;
	}

LPCTSTR PszTraceMaterial(const D3DMATERIAL9* pmaterial)
	{
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%08X->[ [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f %1.2f] [%1.2f %1.2f %1.2f %1.2f] %0.2f ]"), 
		pmaterial, 
		(double)pmaterial->Diffuse.r, (double)pmaterial->Diffuse.g, (double)pmaterial->Diffuse.b, (double)pmaterial->Diffuse.a, 
		(double)pmaterial->Ambient.r, (double)pmaterial->Ambient.g, (double)pmaterial->Ambient.b, (double)pmaterial->Ambient.a, 
		(double)pmaterial->Specular.r, (double)pmaterial->Specular.g, (double)pmaterial->Specular.b, (double)pmaterial->Specular.a, 
		(double)pmaterial->Emissive.r, (double)pmaterial->Emissive.g, (double)pmaterial->Emissive.b, (double)pmaterial->Emissive.a, 
		(double)pmaterial->Power 
		);
	return szBuf;
	}

LPCTSTR PszTraceMatrix(const D3DMATRIX* pmat)
	{
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%08X->[ [%6.1f %6.1f %6.1f %6.1f] [%6.1f %6.1f %6.1f %6.1f] [%6.1f %6.1f %6.1f %6.1f] [%6.1f %6.1f %6.1f %6.1f] ]"), 
		pmat, 
		pmat->_11, pmat->_12, pmat->_13, pmat->_14, 
		pmat->_21, pmat->_22, pmat->_23, pmat->_24, 
		pmat->_31, pmat->_32, pmat->_33, pmat->_34, 
		pmat->_41, pmat->_42, pmat->_43, pmat->_44 
		);
	return szBuf;
	}

LPCTSTR PszTraceViewport(const D3DVIEWPORT9* pviewport)
	{
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%08X->[%u %u %u %u %0.2f %0.2f]"), 
		pviewport, 
		pviewport->X, pviewport->Y, pviewport->Width, pviewport->Height, 
		(double)pviewport->MinZ, (double)pviewport->MaxZ 
		);
	return szBuf;
	}

LPCTSTR PszTraceRenderState(D3DRENDERSTATETYPE rs)
	{
	static LPCTSTR mprspsz[210];
	if (mprspsz[D3DRS_ZENABLE] == NULL)
		{
		mprspsz[D3DRS_ZENABLE] = _T("D3DRS_ZENABLE");
		mprspsz[D3DRS_FILLMODE] = _T("D3DRS_FILLMODE");
		mprspsz[D3DRS_SHADEMODE] = _T("D3DRS_SHADEMODE");
		mprspsz[D3DRS_ZWRITEENABLE] = _T("D3DRS_ZWRITEENABLE");
		mprspsz[D3DRS_ALPHATESTENABLE] = _T("D3DRS_ALPHATESTENABLE");
		mprspsz[D3DRS_LASTPIXEL] = _T("D3DRS_LASTPIXEL");
		mprspsz[D3DRS_SRCBLEND] = _T("D3DRS_SRCBLEND");
		mprspsz[D3DRS_DESTBLEND] = _T("D3DRS_DESTBLEND");
		mprspsz[D3DRS_CULLMODE] = _T("D3DRS_CULLMODE");
		mprspsz[D3DRS_ZFUNC] = _T("D3DRS_ZFUNC");
		mprspsz[D3DRS_ALPHAREF] = _T("D3DRS_ALPHAREF");
		mprspsz[D3DRS_ALPHAFUNC] = _T("D3DRS_ALPHAFUNC");
		mprspsz[D3DRS_DITHERENABLE] = _T("D3DRS_DITHERENABLE");
		mprspsz[D3DRS_ALPHABLENDENABLE] = _T("D3DRS_ALPHABLENDENABLE");
		mprspsz[D3DRS_FOGENABLE] = _T("D3DRS_FOGENABLE");
		mprspsz[D3DRS_SPECULARENABLE] = _T("D3DRS_SPECULARENABLE");
		mprspsz[D3DRS_FOGCOLOR] = _T("D3DRS_FOGCOLOR");
		mprspsz[D3DRS_FOGTABLEMODE] = _T("D3DRS_FOGTABLEMODE");
		mprspsz[D3DRS_FOGSTART] = _T("D3DRS_FOGSTART");
		mprspsz[D3DRS_FOGEND] = _T("D3DRS_FOGEND");
		mprspsz[D3DRS_FOGDENSITY] = _T("D3DRS_FOGDENSITY");
		mprspsz[D3DRS_RANGEFOGENABLE] = _T("D3DRS_RANGEFOGENABLE");
		mprspsz[D3DRS_STENCILENABLE] = _T("D3DRS_STENCILENABLE");
		mprspsz[D3DRS_STENCILFAIL] = _T("D3DRS_STENCILFAIL");
		mprspsz[D3DRS_STENCILZFAIL] = _T("D3DRS_STENCILZFAIL");
		mprspsz[D3DRS_STENCILPASS] = _T("D3DRS_STENCILPASS");
		mprspsz[D3DRS_STENCILFUNC] = _T("D3DRS_STENCILFUNC");
		mprspsz[D3DRS_STENCILREF] = _T("D3DRS_STENCILREF");
		mprspsz[D3DRS_STENCILMASK] = _T("D3DRS_STENCILMASK");
		mprspsz[D3DRS_STENCILWRITEMASK] = _T("D3DRS_STENCILWRITEMASK");
		mprspsz[D3DRS_TEXTUREFACTOR] = _T("D3DRS_TEXTUREFACTOR");
		mprspsz[D3DRS_WRAP0] = _T("D3DRS_WRAP0");
		mprspsz[D3DRS_WRAP1] = _T("D3DRS_WRAP1");
		mprspsz[D3DRS_WRAP2] = _T("D3DRS_WRAP2");
		mprspsz[D3DRS_WRAP3] = _T("D3DRS_WRAP3");
		mprspsz[D3DRS_WRAP4] = _T("D3DRS_WRAP4");
		mprspsz[D3DRS_WRAP5] = _T("D3DRS_WRAP5");
		mprspsz[D3DRS_WRAP6] = _T("D3DRS_WRAP6");
		mprspsz[D3DRS_WRAP7] = _T("D3DRS_WRAP7");
		mprspsz[D3DRS_CLIPPING] = _T("D3DRS_CLIPPING");
		mprspsz[D3DRS_LIGHTING] = _T("D3DRS_LIGHTING");
		mprspsz[D3DRS_AMBIENT] = _T("D3DRS_AMBIENT");
		mprspsz[D3DRS_FOGVERTEXMODE] = _T("D3DRS_FOGVERTEXMODE");
		mprspsz[D3DRS_COLORVERTEX] = _T("D3DRS_COLORVERTEX");
		mprspsz[D3DRS_LOCALVIEWER] = _T("D3DRS_LOCALVIEWER");
		mprspsz[D3DRS_NORMALIZENORMALS] = _T("D3DRS_NORMALIZENORMALS");
		mprspsz[D3DRS_DIFFUSEMATERIALSOURCE] = _T("D3DRS_DIFFUSEMATERIALSOURCE");
		mprspsz[D3DRS_SPECULARMATERIALSOURCE] = _T("D3DRS_SPECULARMATERIALSOURCE");
		mprspsz[D3DRS_AMBIENTMATERIALSOURCE] = _T("D3DRS_AMBIENTMATERIALSOURCE");
		mprspsz[D3DRS_EMISSIVEMATERIALSOURCE] = _T("D3DRS_EMISSIVEMATERIALSOURCE");
		mprspsz[D3DRS_VERTEXBLEND] = _T("D3DRS_VERTEXBLEND");
		mprspsz[D3DRS_CLIPPLANEENABLE] = _T("D3DRS_CLIPPLANEENABLE");
		mprspsz[D3DRS_POINTSIZE] = _T("D3DRS_POINTSIZE");
		mprspsz[D3DRS_POINTSIZE_MIN] = _T("D3DRS_POINTSIZE_MIN");
		mprspsz[D3DRS_POINTSPRITEENABLE] = _T("D3DRS_POINTSPRITEENABLE");
		mprspsz[D3DRS_POINTSCALEENABLE] = _T("D3DRS_POINTSCALEENABLE");
		mprspsz[D3DRS_POINTSCALE_A] = _T("D3DRS_POINTSCALE_A");
		mprspsz[D3DRS_POINTSCALE_B] = _T("D3DRS_POINTSCALE_B");
		mprspsz[D3DRS_POINTSCALE_C] = _T("D3DRS_POINTSCALE_C");
		mprspsz[D3DRS_MULTISAMPLEANTIALIAS] = _T("D3DRS_MULTISAMPLEANTIALIAS");
		mprspsz[D3DRS_MULTISAMPLEMASK] = _T("D3DRS_MULTISAMPLEMASK");
		mprspsz[D3DRS_PATCHEDGESTYLE] = _T("D3DRS_PATCHEDGESTYLE");
		mprspsz[D3DRS_DEBUGMONITORTOKEN] = _T("D3DRS_DEBUGMONITORTOKEN");
		mprspsz[D3DRS_POINTSIZE_MAX] = _T("D3DRS_POINTSIZE_MAX");
		mprspsz[D3DRS_INDEXEDVERTEXBLENDENABLE] = _T("D3DRS_INDEXEDVERTEXBLENDENABLE");
		mprspsz[D3DRS_COLORWRITEENABLE] = _T("D3DRS_COLORWRITEENABLE");
		mprspsz[D3DRS_TWEENFACTOR] = _T("D3DRS_TWEENFACTOR");
		mprspsz[D3DRS_BLENDOP] = _T("D3DRS_BLENDOP");
		mprspsz[D3DRS_POSITIONDEGREE] = _T("D3DRS_POSITIONDEGREE");
		mprspsz[D3DRS_NORMALDEGREE] = _T("D3DRS_NORMALDEGREE");
		mprspsz[D3DRS_SCISSORTESTENABLE] = _T("D3DRS_SCISSORTESTENABLE");
		mprspsz[D3DRS_SLOPESCALEDEPTHBIAS] = _T("D3DRS_SLOPESCALEDEPTHBIAS");
		mprspsz[D3DRS_ANTIALIASEDLINEENABLE] = _T("D3DRS_ANTIALIASEDLINEENABLE");
		mprspsz[D3DRS_MINTESSELLATIONLEVEL] = _T("D3DRS_MINTESSELLATIONLEVEL");
		mprspsz[D3DRS_MAXTESSELLATIONLEVEL] = _T("D3DRS_MAXTESSELLATIONLEVEL");
		mprspsz[D3DRS_ADAPTIVETESS_X] = _T("D3DRS_ADAPTIVETESS_X");
		mprspsz[D3DRS_ADAPTIVETESS_Y] = _T("D3DRS_ADAPTIVETESS_Y");
		mprspsz[D3DRS_ADAPTIVETESS_Z] = _T("D3DRS_ADAPTIVETESS_Z");
		mprspsz[D3DRS_ADAPTIVETESS_W] = _T("D3DRS_ADAPTIVETESS_W");
		mprspsz[D3DRS_ENABLEADAPTIVETESSELLATION] = _T("D3DRS_ENABLEADAPTIVETESSELLATION");
		mprspsz[D3DRS_TWOSIDEDSTENCILMODE] = _T("D3DRS_TWOSIDEDSTENCILMODE");
		mprspsz[D3DRS_CCW_STENCILFAIL] = _T("D3DRS_CCW_STENCILFAIL");
		mprspsz[D3DRS_CCW_STENCILZFAIL] = _T("D3DRS_CCW_STENCILZFAIL");
		mprspsz[D3DRS_CCW_STENCILPASS] = _T("D3DRS_CCW_STENCILPASS");
		mprspsz[D3DRS_CCW_STENCILFUNC] = _T("D3DRS_CCW_STENCILFUNC");
		mprspsz[D3DRS_COLORWRITEENABLE1] = _T("D3DRS_COLORWRITEENABLE1");
		mprspsz[D3DRS_COLORWRITEENABLE2] = _T("D3DRS_COLORWRITEENABLE2");
		mprspsz[D3DRS_COLORWRITEENABLE3] = _T("D3DRS_COLORWRITEENABLE3");
		mprspsz[D3DRS_BLENDFACTOR] = _T("D3DRS_BLENDFACTOR");
		mprspsz[D3DRS_SRGBWRITEENABLE] = _T("D3DRS_SRGBWRITEENABLE");
		mprspsz[D3DRS_DEPTHBIAS] = _T("D3DRS_DEPTHBIAS");
		mprspsz[D3DRS_WRAP8] = _T("D3DRS_WRAP8");
		mprspsz[D3DRS_WRAP9] = _T("D3DRS_WRAP9");
		mprspsz[D3DRS_WRAP10] = _T("D3DRS_WRAP10");
		mprspsz[D3DRS_WRAP11] = _T("D3DRS_WRAP11");
		mprspsz[D3DRS_WRAP12] = _T("D3DRS_WRAP12");
		mprspsz[D3DRS_WRAP13] = _T("D3DRS_WRAP13");
		mprspsz[D3DRS_WRAP14] = _T("D3DRS_WRAP14");
		mprspsz[D3DRS_WRAP15] = _T("D3DRS_WRAP15");
		mprspsz[D3DRS_SEPARATEALPHABLENDENABLE] = _T("D3DRS_SEPARATEALPHABLENDENABLE");
		mprspsz[D3DRS_SRCBLENDALPHA] = _T("D3DRS_SRCBLENDALPHA");
		mprspsz[D3DRS_DESTBLENDALPHA] = _T("D3DRS_DESTBLENDALPHA");
		mprspsz[D3DRS_BLENDOPALPHA] = _T("D3DRS_BLENDOPALPHA");
		}
	
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%u: %s"), rs, mprspsz[rs]);
	return szBuf;
	}

LPCTSTR PszTraceSamplerState(D3DSAMPLERSTATETYPE ss)
	{
	static LPCTSTR mpsspsz[] = 
		{
		NULL,
		_T("D3DSAMP_ADDRESSU"),
		_T("D3DSAMP_ADDRESSV"),
		_T("D3DSAMP_ADDRESSW"),
		_T("D3DSAMP_BORDERCOLOR"),
		_T("D3DSAMP_MAGFILTER"),
		_T("D3DSAMP_MINFILTER"),
		_T("D3DSAMP_MIPFILTER"),
		_T("D3DSAMP_MIPMAPLODBIAS"),
		_T("D3DSAMP_MAXMIPLEVEL"),
		_T("D3DSAMP_MAXANISOTROPY"),
		_T("D3DSAMP_SRGBTEXTURE"),
		_T("D3DSAMP_ELEMENTINDEX"),
		_T("D3DSAMP_DMAPOFFSET"),
 		};
	
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%u: %s"), ss, mpsspsz[ss]);
	return szBuf;
	}

LPCTSTR PszTraceTextureStageState(D3DTEXTURESTAGESTATETYPE tss)
	{
	static LPCTSTR mptsspsz[33];
	if (mptsspsz[D3DTSS_COLOROP] == NULL)
		{
		mptsspsz[D3DTSS_COLOROP] = _T("D3DTSS_COLOROP");
		mptsspsz[D3DTSS_COLORARG1] = _T("D3DTSS_COLORARG1");
		mptsspsz[D3DTSS_COLORARG2] = _T("D3DTSS_COLORARG2");
		mptsspsz[D3DTSS_ALPHAOP] = _T("D3DTSS_ALPHAOP");
		mptsspsz[D3DTSS_ALPHAARG1] = _T("D3DTSS_ALPHAARG1");
		mptsspsz[D3DTSS_ALPHAARG2] = _T("D3DTSS_ALPHAARG2");
		mptsspsz[D3DTSS_BUMPENVMAT00] = _T("D3DTSS_BUMPENVMAT00");
		mptsspsz[D3DTSS_BUMPENVMAT01] = _T("D3DTSS_BUMPENVMAT01");
		mptsspsz[D3DTSS_BUMPENVMAT10] = _T("D3DTSS_BUMPENVMAT10");
		mptsspsz[D3DTSS_BUMPENVMAT11] = _T("D3DTSS_BUMPENVMAT11");
		mptsspsz[D3DTSS_TEXCOORDINDEX] = _T("D3DTSS_TEXCOORDINDEX");
		mptsspsz[D3DTSS_BUMPENVLSCALE] = _T("D3DTSS_BUMPENVLSCALE");
		mptsspsz[D3DTSS_BUMPENVLOFFSET] = _T("D3DTSS_BUMPENVLOFFSET");
		mptsspsz[D3DTSS_TEXTURETRANSFORMFLAGS] = _T("D3DTSS_TEXTURETRANSFORMFLAGS");
		mptsspsz[D3DTSS_COLORARG0] = _T("D3DTSS_COLORARG0");
		mptsspsz[D3DTSS_ALPHAARG0] = _T("D3DTSS_ALPHAARG0");
		mptsspsz[D3DTSS_RESULTARG] = _T("D3DTSS_RESULTARG");
		mptsspsz[D3DTSS_CONSTANT] = _T("D3DTSS_CONSTANT");
 		};
	
	static TCHAR szBuf[200];
	_sntprintf_s(szBuf, sizeof(szBuf)/sizeof(TCHAR), _TRUNCATE, 
		_T("%u: %s"), tss, mptsspsz[tss]);
	return szBuf;
	}

void DumpMatrix(const D3DMATRIX* pmat)
	{
	HexDump((const DWORD*)(&pmat->_11), 4, 0);
	HexDump((const DWORD*)(&pmat->_21), 4, 0);
	HexDump((const DWORD*)(&pmat->_31), 4, 0);
	HexDump((const DWORD*)(&pmat->_41), 4, 0);
	}

void HexDump(const DWORD* prgdw, UINT cfloat, UINT cdw)
	{
	TCHAR szLine[1000];
	TCHAR* pszBuf = szLine;
	UINT cchBuf = sizeof(szLine)/sizeof(TCHAR);
	
	UINT cch = _sntprintf_s(pszBuf, cchBuf, _TRUNCATE, _T("%08X: "), prgdw);
	pszBuf += cch;
	cchBuf -= cch;
	
	for (UINT ifloat = 0; ifloat < cfloat; ifloat++)
		{
		cch = _sntprintf_s(pszBuf, cchBuf, _TRUNCATE, _T(" %8.1f"), 
			(double)(((float*)prgdw)[ifloat]));
		pszBuf += cch;
		cchBuf -= cch;
		}
	
	for (UINT idw = 0; idw < cdw; idw++)
		{
		cch = _sntprintf_s(pszBuf, cchBuf, _TRUNCATE, _T(" %08X"), prgdw[cfloat + idw]);
		pszBuf += cch;
		cchBuf -= cch;
		}

	_tcscpy_s(pszBuf, cchBuf, _T("\n"));
	OutputDebugString(szLine);
	}

void DebugLine(LPCTSTR psz, ...)
	{
	va_list args;
	va_start(args, psz);
	TCHAR sz[2000];
	_vsntprintf_s(sz, sizeof(sz)/sizeof(TCHAR), _TRUNCATE, psz, args);
	_tcscat_s(sz, sizeof(sz)/sizeof(TCHAR), _T("\n"));
	OutputDebugString(sz);
	va_end(args);
	}


