// SVRenderHook.h : Declaration of the CSVRenderHook

#pragma once
#include "resource.h"       // main symbols

#include "RenderHook.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif


#pragma pack(push)
#pragma pack(1)
struct SLB	// Short Landblock
	{
	union
		{
		WORD w;
		struct
			{
			BYTE ilbLat;
			BYTE ilbLng;
			};
		};
	};

struct LANDBLOCK
	{
	union
		{
		DWORD dw;
		struct
			{
			WORD iblock;
			SLB slb;
			};
		};
	};

struct FRAME
		{
/*00*/	DWORD pvtable;
/*04*/	LANDBLOCK landblock;
/*08*/	float quat[4];
/*18*/	float mat[9];
/*3C*/	float x;
/*40*/	float y;
/*44*/	float z;
		};

struct ARGB
	{
	union
		{
		DWORD dw;
		struct
			{
			BYTE b, g, r, a;
			};
		};

	ARGB(DWORD dwArg)
		{
		dw = dwArg;
		}
	};
#pragma pack(pop)

enum KPASS
	{
	kpassUnknown,
	kpassYes,
	kpassNo
	};


// CSVRenderHook
class ATL_NO_VTABLE CSVRenderHook :
	public CComObjectRootEx<CComSingleThreadModel>, 
	public CComCoClass<CSVRenderHook, &CLSID_SVRenderHook>, 
	public IDispatchImpl<ISVRenderHook, &IID_ISVRenderHook, &LIBID_RenderHookLib, /*wMajor =*/ 1, /*wMinor =*/ 0>, 
	public IRender3DSink
{
public:
	CSVRenderHook() : 
	  m_fEnabled(false), 
	  m_fSlope(false), 
	  m_fWater(false), 
	  m_fLight(false), 
	  m_argbSlope(0), 
	  m_argbWater(0), 
	  m_argbLight(0), 
	  m_fTrace(false), 
	  m_fHooked(false)
		{
		}

	~CSVRenderHook()
		{
		}

	DECLARE_REGISTRY_RESOURCEID(IDR_SVRENDERHOOK)

	BEGIN_COM_MAP(CSVRenderHook)
		COM_INTERFACE_ENTRY(ISVRenderHook)
		COM_INTERFACE_ENTRY(IDispatch)
		COM_INTERFACE_ENTRY(IRender3DSink)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
		{
		return S_OK;
		}

	void FinalRelease()
		{
		}

//	IRender3DSink methods:
public:
	STDMETHOD(PreBeginScene)(LPUNKNOWN pD3D);
	STDMETHOD(PostBeginScene)(LPUNKNOWN pD3D);
	STDMETHOD(PreEndScene)(LPUNKNOWN pD3D);
	STDMETHOD(PostEndScene)(LPUNKNOWN pD3D);

//	ISVRenderHook Methods:
public:
	STDMETHOD(Init)(IUnknown* punkNetSvc);
	STDMETHOD(Finalize)(void);
	STDMETHOD(get_fEnabled)(VARIANT_BOOL* pVal);
	STDMETHOD(put_fEnabled)(VARIANT_BOOL newVal);
	STDMETHOD(get_fSlope)(VARIANT_BOOL* pVal);
	STDMETHOD(put_fSlope)(VARIANT_BOOL newVal);
	STDMETHOD(get_fWater)(VARIANT_BOOL* pVal);
	STDMETHOD(put_fWater)(VARIANT_BOOL newVal);
	STDMETHOD(get_fLight)(VARIANT_BOOL* pVal);
	STDMETHOD(put_fLight)(VARIANT_BOOL newVal);
	STDMETHOD(get_colorSlope)(LONG* pVal);
	STDMETHOD(put_colorSlope)(LONG newVal);
	STDMETHOD(get_colorWater)(LONG* pVal);
	STDMETHOD(put_colorWater)(LONG newVal);
	STDMETHOD(get_colorLight)(LONG* pVal);
	STDMETHOD(put_colorLight)(LONG newVal);
	STDMETHOD(get_fTrace)(VARIANT_BOOL* pVal);
	STDMETHOD(put_fTrace)(VARIANT_BOOL newVal);
	STDMETHOD(TraceOneFrame)(void);

private:
	Decal::IACHooksPtr m_phooks;
	DecalDat::IDatLibraryPtr m_pcell;

	BOOL m_fEnabled;
	BOOL m_fSlope;
	BOOL m_fWater;
	BOOL m_fLight;
	ARGB m_argbSlope;
	ARGB m_argbWater;
	ARGB m_argbLight;
	BOOL m_fTrace;
	BOOL m_fHooked;

	BOOL FNeedHook();
	KPASS KpassFromSlb(SLB slb);
};

OBJECT_ENTRY_AUTO(__uuidof(SVRenderHook), CSVRenderHook)
