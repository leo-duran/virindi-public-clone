// SVRenderHook.cpp : Implementation of CSVRenderHook

#include "stdafx.h"
#include "Trace.h"
#include "SVRenderHook.h"

void HookAllMethods(IDirect3DDevice9* pD3DD9);
void UnhookAllMethods(IDirect3DDevice9* pD3DD9);

void HookMethods( IDirect3DDevice9* pD3DD9 );
void UnhookMethods( IDirect3DDevice9* pD3DD9 );

HRESULT (STDMETHODCALLTYPE *SetTransform0)(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State,CONST D3DMATRIX* pMatrix) = NULL;
HRESULT STDMETHODCALLTYPE SetTransformH(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State,CONST D3DMATRIX* pMatrix);

HRESULT (STDMETHODCALLTYPE *DrawPrimitiveUP0)(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType,UINT PrimitiveCount,CONST void* pVertexStreamZeroData,UINT VertexStreamZeroStride) = NULL;
HRESULT STDMETHODCALLTYPE DrawPrimitiveUPH(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType,UINT PrimitiveCount,CONST void* pVertexStreamZeroData,UINT VertexStreamZeroStride);

HRESULT (STDMETHODCALLTYPE *SetFVFO)(IDirect3DDevice9* pD3DD9, DWORD FVF) = NULL;
HRESULT STDMETHODCALLTYPE SetFVFH(IDirect3DDevice9* pD3DD9, DWORD FVF);

HRESULT (STDMETHODCALLTYPE *SetLightO)(IDirect3DDevice9* pD3DD9, DWORD Index, CONST D3DLIGHT9* pLight) = NULL;
HRESULT STDMETHODCALLTYPE SetLightH(IDirect3DDevice9* pD3DD9, DWORD Index, CONST D3DLIGHT9* pLight);

BOOL s_fSlope, s_fWater, s_fLight;
ARGB s_argbSlope(0), s_argbWater(0), s_argbLight(0);

FRAME* pframeCamera;

BYTE mpslbkpass[0x10000];

BOOL fTraceOneFrame = false;

D3DMATRIX mWorld;
DWORD fvfCur;


///////////////////////////
// ISVRenderHook methods //
///////////////////////////

STDMETHODIMP CSVRenderHook::Init(IUnknown* punkNetSvc)
	{
	if (m_fTrace) DebugLine(_T("[ CSVRenderHook::Init"));
	HRESULT hr;

//	Resolve punkNetSvc to INetService.
//	REVIEW: Figure out how to pass it in as this type.
	DecalNet::INetServicePtr pnetsvc;
	if (m_fTrace) DebugLine(_T("  QueryInterface(INetService)"));
	hr = punkNetSvc->QueryInterface<DecalNet::INetService>(&pnetsvc);
	if (FAILED(hr)) return hr;

//	Get a ref to the Decal object.
	Decal::IDecalPtr pdecal;
	if (m_fTrace) DebugLine(_T("  pnetsvc->get_Decal"));
	hr = pnetsvc->get_Decal(&pdecal);
	if (FAILED(hr)) return hr;

//	Get a ref to ACHooks.
	if (m_fTrace) DebugLine(_T("  pdecal->get_Hooks"));
	hr = pdecal->get_Hooks(&m_phooks);
	if (FAILED(hr)) return hr;

//	Get a ref to the Inject service.
	Decal::IInjectServicePtr pinject;
	if (m_fTrace) DebugLine(_T("  pdecal->get_Object(inject)"));
	hr = pdecal->get_Object(_T("services\\DecalPlugins.InjectService"), 
		(GUID*)&__uuidof(Decal::IInjectService), reinterpret_cast<IUnknown**>(&pinject));
	if (FAILED(hr))
		DebugLine(_T("  get_Object failed (%08X)."), hr);

//	Get a ref to me as IUnknown.
//	REVIEW: Is there an easier/better way to to this?
	IUnknownPtr punkThis;
	if (m_fTrace) DebugLine(_T("  QueryInterface(IUnknown)"));
	hr = QueryInterface(__uuidof(IUnknown), 
		reinterpret_cast<void**>(&punkThis));
	if (FAILED(hr))
		DebugLine(_T("  QueryInterface failed (%08X)."), hr);

//	Hook up IRender3DSink callbacks.
	if (m_fTrace) DebugLine(_T("  pinject->InitPlugin"));
	hr = pinject->InitPlugin(punkThis);
	if (FAILED(hr))
		DebugLine(_T("  InitPlugin failed (%08X)."), hr);
	
//	Get a ref to Cell.dat.
	if (m_fTrace) DebugLine(_T("  pdecal->get_Object(cell)"));
	hr = pdecal->get_Object(_T("services\\DecalDat.DatService\\cell"), 
		(GUID*)&__uuidof(DecalDat::IDatLibrary), reinterpret_cast<IUnknown**>(&m_pcell));
	if (FAILED(hr)) return hr;
	
	if (m_fTrace) DebugLine(_T("] CSVRenderHook::Init"));
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::Finalize(void)
	{
	if (m_fTrace) DebugLine(_T("[ CSVRenderHook::Finalize"));
	HRESULT hr;

	m_phooks = NULL;

//	Release Cell.dat.
	m_pcell = NULL;
	
	if (m_fTrace) DebugLine(_T("] CSVRenderHook::Finalize"));
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_fEnabled(VARIANT_BOOL* pVal)
	{
	*pVal = m_fEnabled;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_fEnabled(VARIANT_BOOL newVal)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::put_fEnabled(%s)"), 
		newVal ? _T("true") : __T("false"));

	m_fEnabled = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_fSlope(VARIANT_BOOL* pVal)
	{
	*pVal = m_fSlope;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_fSlope(VARIANT_BOOL newVal)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::put_fSlope(%s)"), 
		newVal ? _T("true") : __T("false"));
	
	m_fSlope = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_fWater(VARIANT_BOOL* pVal)
	{
	*pVal = m_fWater;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_fWater(VARIANT_BOOL newVal)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::put_fWater(%s)"), 
		newVal ? _T("true") : __T("false"));

	m_fWater = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_fLight(VARIANT_BOOL* pVal)
	{
	*pVal = m_fLight;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_fLight(VARIANT_BOOL newVal)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::put_fLight(%s)"), 
		newVal ? _T("true") : __T("false"));
	
	m_fLight = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_colorSlope(LONG* pVal)
	{
	*pVal = m_argbSlope.dw;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_colorSlope(LONG newVal)
	{
	//if (m_fTrace) DebugLine(_T("CSVRenderHook::put_colorSlope(%08X)"), newVal);

	m_argbSlope.dw = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_colorWater(LONG* pVal)
	{
	*pVal = m_argbWater.dw;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_colorWater(LONG newVal)
	{
	//if (m_fTrace) DebugLine(_T("CSVRenderHook::put_colorWater(%08X)"), newVal);

	m_argbWater.dw = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_colorLight(LONG* pVal)
	{
	*pVal = m_argbLight.dw;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_colorLight(LONG newVal)
	{
	//if (m_fTrace) DebugLine(_T("CSVRenderHook::put_colorLight(%08X)"), newVal);

	m_argbLight.dw = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::get_fTrace(VARIANT_BOOL* pVal)
	{
	*pVal = m_fTrace;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::put_fTrace(VARIANT_BOOL newVal)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::put_fTrace(%s)"), 
		newVal ? _T("true") : __T("false"));
	
	m_fTrace = newVal;
	return S_OK;
	}

STDMETHODIMP CSVRenderHook::TraceOneFrame(void)
	{
	if (m_fTrace) DebugLine(_T("CSVRenderHook::TraceOneFrame"));

	fTraceOneFrame = true;
	return S_OK;
	}


///////////////////////////
// IRender3DSink methods //
///////////////////////////

STDMETHODIMP CSVRenderHook::PreBeginScene(LPUNKNOWN pD3D)
	{
	IDirect3DDevice9* pD3DD9 = (IDirect3DDevice9*)pD3D;
	if (fTraceOneFrame) DebugLine(_T("CSVRenderHook::PreBeginScene"));

	if (FNeedHook() /* && !m_fHooked */)
		{
	//	Need vtable hooks but don't have them.
		if (m_fTrace && !m_fHooked) DebugLine(_T("SVRenderHook: Hooking vtable..."));
		HookMethods(pD3DD9);
		m_fHooked = true;
		}

	if (fTraceOneFrame)
		HookAllMethods(pD3DD9);

//	Make member vars statically available to IDirect3DDevice9 hook functions.
	s_fSlope = m_fSlope;
	s_fWater = m_fWater;
	s_fLight = m_fLight;
	s_argbSlope = m_argbSlope;
	s_argbWater = m_argbWater;
	s_argbLight = m_argbLight;

	if (m_fWater)
		{
	//	Note the camera's landblock for use in calculating terrain coords.
		pframeCamera = (FRAME*)(m_phooks->SmartboxPtr() + 0x08);
		SLB slbCamera = pframeCamera->landblock.slb;
		if (fTraceOneFrame)
			DebugLine(_T("  landblockCamera = %04X"), slbCamera.w);
		
	//	Preload kpass table for neighboring landblocks.
		for (int dilbLng = -3; dilbLng <= 3; dilbLng++)
			{
			int ilbLng = slbCamera.ilbLng + dilbLng;
			if (ilbLng >= 0 && ilbLng <= 0xFE)
				for (int dilbLat = -3; dilbLat <= 3; dilbLat++)
					{
					int ilbLat = slbCamera.ilbLat + dilbLat;
					if (ilbLat >= 0 && ilbLat <= 0xFE)
						{
						SLB slb;
						slb.ilbLng = ilbLng;
						slb.ilbLat = ilbLat;
						if (mpslbkpass[slb.w] == kpassUnknown)
							mpslbkpass[slb.w] = KpassFromSlb(slb);
						}
					}
			}
		}

	return S_OK;
	}

STDMETHODIMP CSVRenderHook::PostBeginScene(LPUNKNOWN pD3D)
	{
	IDirect3DDevice9* pD3DD9 = (IDirect3DDevice9*)pD3D;
	if (fTraceOneFrame) DebugLine(_T("CSVRenderHook::PostBeginScene"));

	return S_OK;
	}

STDMETHODIMP CSVRenderHook::PreEndScene(LPUNKNOWN pD3D)
	{
	IDirect3DDevice9* pD3DD9 = (IDirect3DDevice9*)pD3D;
	if (fTraceOneFrame) DebugLine(_T("CSVRenderHook::PreEndScene"));

	return S_OK;
	}

STDMETHODIMP CSVRenderHook::PostEndScene(LPUNKNOWN pD3D)
	{
	IDirect3DDevice9* pD3DD9 = (IDirect3DDevice9*)pD3D;
	if (fTraceOneFrame) DebugLine(_T("CSVRenderHook::PostEndScene"));

	if (fTraceOneFrame)
		{
		UnhookAllMethods(pD3DD9);
		fTraceOneFrame = false;
		}

	if (m_fHooked && !FNeedHook())
		{
	//	Done with vtable hooks; remove them.
		if (m_fTrace) DebugLine(_T("SVRenderHook: Unhooking vtable..."));
		UnhookMethods(pD3DD9);
		m_fHooked = false;
		}
	
	return S_OK;
	}


////////////////////////////////////////////
// IDirect3DDevice9 vtable hook functions //
////////////////////////////////////////////

HRESULT STDMETHODCALLTYPE SetTransformH(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State,CONST D3DMATRIX* pMatrix)
	{
	if (s_fWater && State == D3DTS_WORLD)
		{
	//	Water highlight needs World transform to calculate landblock of rendered triangles.
		mWorld = *pMatrix;
		}

	return SetTransform0(pD3DD9, State, pMatrix);;
	}

HRESULT STDMETHODCALLTYPE SetFVFH(IDirect3DDevice9* pD3DD9, DWORD FVF)
	{
	fvfCur = FVF;

	return SetFVFO(pD3DD9, FVF);
	}

#pragma pack(push)
#pragma pack(1)
// AC's terrain vertex format:
struct TVTX
	{
	float x, y, z;		// Landblock-relative coords
	ARGB argb;			// Diffuse color
//	float tu, tv;		// Texture coords
	};
#pragma pack(pop)

__inline void BlendArgb(ARGB& argbDst, ARGB argbSrc)
	{
	float fractSrc = (float)argbSrc.a/255;
	float fractDst = (float)1.0 - fractSrc;
	argbDst.r = (BYTE)(argbDst.r * fractDst + argbSrc.r * fractSrc);
	argbDst.g = (BYTE)(argbDst.g * fractDst + argbSrc.g * fractSrc);
	argbDst.b = (BYTE)(argbDst.b * fractDst + argbSrc.b * fractSrc);
	if (fTraceOneFrame)
		DebugLine(_T("fractSrc = %02f, fractDst = %02f, argbSrc = %08X, argbDst = %08X"), 
		  fractSrc, fractDst, argbSrc, argbDst);
	}

HRESULT STDMETHODCALLTYPE DrawPrimitiveUPH(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType,UINT PrimitiveCount,CONST void* pVertexStreamZeroData,UINT VertexStreamZeroStride)
	{
	if (PrimitiveType == D3DPT_TRIANGLELIST)
		{
	//	We happen to know that DrawPrimitiveUP is used only for terrain.
	//	Check each triangle to see if it should be highlighted.
		BYTE* prgbTvtx = (BYTE*)pVertexStreamZeroData;
		for (UINT iprim = 0; iprim < PrimitiveCount; iprim++)
			{
			TVTX* ptvtx1 = (TVTX*)&prgbTvtx[(iprim * 3 + 0) * VertexStreamZeroStride];
			TVTX* ptvtx2 = (TVTX*)&prgbTvtx[(iprim * 3 + 1) * VertexStreamZeroStride];
			TVTX* ptvtx3 = (TVTX*)&prgbTvtx[(iprim * 3 + 2) * VertexStreamZeroStride];
			bool fHighlighted = false;

			if (s_fWater)
				{
				SLB slb = pframeCamera->landblock.slb;
				slb.ilbLng += (BYTE)(mWorld._41/192);
				slb.ilbLat += (BYTE)(mWorld._43/192);
				if (mpslbkpass[slb.w] == kpassNo)
					{
					BlendArgb(ptvtx1->argb, s_argbWater);
					BlendArgb(ptvtx2->argb, s_argbWater);
					BlendArgb(ptvtx3->argb, s_argbWater);
					fHighlighted = true;
					}
				}
			if (s_fSlope && ! fHighlighted)
				{
			//	Calculate the X and Y components of the slope.
				float dzDx, dzDy;
				if (ptvtx1->y == ptvtx2->y)
					dzDx = (ptvtx1->z - ptvtx2->z)/(ptvtx1->x - ptvtx2->x);
				else if (ptvtx1->y == ptvtx3->y)
					dzDx = (ptvtx1->z - ptvtx3->z)/(ptvtx1->x - ptvtx3->x);
				else
					dzDx = (ptvtx2->z - ptvtx3->z)/(ptvtx2->x - ptvtx3->x);
			    
				if (ptvtx1->x == ptvtx2->x)
					dzDy = (ptvtx1->z - ptvtx2->z)/(ptvtx1->y - ptvtx2->y);
				else if (ptvtx1->x == ptvtx3->x)
					dzDy = (ptvtx1->z - ptvtx3->z)/(ptvtx1->y - ptvtx3->y);
				else
					dzDy = (ptvtx2->z - ptvtx3->z)/(ptvtx2->y - ptvtx3->y);
			    
			//	If the Pythagorean sum of those components exceeds 27/24 (1.125), 
			//	then the slope is too steep to climb.
				if (dzDx*dzDx + dzDy*dzDy > 1.265625)
					{
					BlendArgb(ptvtx1->argb, s_argbSlope);
					BlendArgb(ptvtx2->argb, s_argbSlope);
					BlendArgb(ptvtx3->argb, s_argbSlope);
					fHighlighted = true;
					}
				}
			}
		}
	
	return DrawPrimitiveUP0(pD3DD9, 
		PrimitiveType, PrimitiveCount, pVertexStreamZeroData, VertexStreamZeroStride);
	}

HRESULT STDMETHODCALLTYPE SetLightH(IDirect3DDevice9* pD3DD9, DWORD Index, CONST D3DLIGHT9* pLight)
    {
	if (s_fLight && Index == 0 && pLight->Type == 1)
		{
		D3DLIGHT9 lightNew = *pLight;
		lightNew.Ambient.r = s_argbLight.r/10;
		lightNew.Ambient.g = s_argbLight.g/10;
		lightNew.Ambient.b = s_argbLight.b/10;
		lightNew.Range = 100 * s_argbLight.a/255;
		if (fTraceOneFrame) DebugLine(_T("  Adjusted:        %s"), PszTraceLight(&lightNew));
		return SetLightO(pD3DD9, Index, &lightNew);
		}
	else
		return SetLightO(pD3DD9, Index, pLight);
    }


//////////////
// Privates //
//////////////

BOOL CSVRenderHook::FNeedHook()
	{
	return m_fEnabled && (m_fSlope || m_fWater || m_fLight);
	}

KPASS CSVRenderHook::KpassFromSlb(SLB slb)
	{
	if (m_fTrace) DebugLine(_T("Loading terrain data for landblock %04X..."), slb.w);

	IUnknownPtr pUnk;
	HRESULT hr = m_pcell->get_Stream(slb.w << 16 | 0xFFFF, &pUnk);
	if (SUCCEEDED(hr))
		{
		DecalDat::IDatStreamPtr pstream;
		hr = pUnk->QueryInterface<DecalDat::IDatStream>(&pstream);
		if (SUCCEEDED(hr))
			{
			pstream->Skip(8);
			WORD rgwTex[9][9];
			hr = pstream->ReadBinary(sizeof(rgwTex), (BYTE*)rgwTex);
			if (SUCCEEDED(hr))
				{
				//if (m_fTrace)
				//	{
				//	for (UINT irow = 0; irow < 9; irow++)
				//		{
				//		TCHAR sz1[50];
				//		TCHAR sz2[50];
				//		for (UINT icol = 0; icol < 9; icol++)
				//			{
				//			WORD wTex = rgwTex[icol][irow];
				//			UINT itex = (wTex >> 2) & 0x3F;
				//			_sntprintf_s(&sz1[icol * 5], 50 - icol * 5, _TRUNCATE, _T("%04X "), wTex);
				//			_sntprintf_s(&sz2[icol * 5], 50 - icol * 5, _TRUNCATE, _T("%4u "), itex);
				//			}
				//		DebugLine(_T("%s: %s"), sz1, sz2);
				//		}
				//	}

				for (UINT irow = 0; irow < 9; irow++)
					for (UINT icol = 0; icol < 9; icol++)
						{
						WORD wTex = rgwTex[icol][irow];
						UINT itex = (wTex >> 2) & 0x3F;
						if (itex < 16 || itex > 20)
							{
							if (m_fTrace) DebugLine(_T("%04X is passable."), slb.w);
							return kpassYes;
							}
						}
					
			//	All water; mark it impassable.
				if (m_fTrace) DebugLine(_T("%04X is impassable."), slb.w);
				return kpassNo;
				}
			}
		}

	DebugLine(_T(">>> Couldn't read terrain data for landblock %04X."), slb.w);
	return kpassYes;
	}


/////////////////////////////////////////
// Ripped from Decal's Direct3D9Hook.h //
/////////////////////////////////////////

LPVOID GetVTableEntry( LPVOID lpObject, ULONG ulIndex )
{
	return ( *( ( LPVOID** )lpObject ) )[ ulIndex ];
}

LPVOID HookVTable( LPVOID lpObject, ULONG ulIndex, LPVOID lpOriginal, LPVOID lpDetour )
	{
	if( GetVTableEntry( lpObject, ulIndex ) != lpDetour )
		{
		LPVOID* lpVTableEntry = &( *( ( LPVOID** )lpObject ) )[ ulIndex ];
		DWORD dwOldProtect;

		VirtualProtect( lpVTableEntry, sizeof( DWORD_PTR ), PAGE_EXECUTE_READWRITE, &dwOldProtect );

		*( ( LPVOID* )lpOriginal ) = *lpVTableEntry;
		*lpVTableEntry = lpDetour;

		VirtualProtect( lpVTableEntry, sizeof( DWORD_PTR ), dwOldProtect, &dwOldProtect );
		FlushInstructionCache( GetCurrentProcess( ), lpVTableEntry, sizeof( DWORD_PTR ) );
		}
	return *( ( LPVOID* )lpOriginal );
	}

LPVOID UnhookVTable( LPVOID lpObject, ULONG ulIndex, LPVOID lpOriginal, LPVOID lpDetour )
	{
	LPVOID lpHooked = GetVTableEntry( lpObject, ulIndex );
	if( lpHooked == lpDetour )
		{
		LPVOID* lpVTableEntry = &(*( ( LPVOID** )lpObject ) )[ ulIndex ];
		DWORD dwOldProtect;
		VirtualProtect( lpVTableEntry, sizeof( DWORD_PTR ), PAGE_EXECUTE_READWRITE, &dwOldProtect );
		*lpVTableEntry = lpOriginal;

		VirtualProtect( lpVTableEntry, sizeof( DWORD_PTR ), dwOldProtect, NULL );
		FlushInstructionCache( GetCurrentProcess( ), lpVTableEntry, sizeof( DWORD_PTR ) );
		}
	return lpHooked;
	}

void HookMethods(IDirect3DDevice9* pD3DD9)
	{
	HookVTable(pD3DD9, 44, &SetTransform0, SetTransformH);
	HookVTable(pD3DD9, 83, &DrawPrimitiveUP0, DrawPrimitiveUPH);
    HookVTable(pD3DD9, 89, &SetFVFO, SetFVFH);
    HookVTable(pD3DD9, 51, &SetLightO, SetLightH);
	}

void UnhookMethods(IDirect3DDevice9* pD3DD9)
	{
	UnhookVTable(pD3DD9, 44, SetTransform0, SetTransformH);
	UnhookVTable(pD3DD9, 83, DrawPrimitiveUP0, DrawPrimitiveUPH);
    UnhookVTable(pD3DD9, 89, SetFVFO, SetFVFH);
    UnhookVTable(pD3DD9, 51, SetLightO, SetLightH);
	}

