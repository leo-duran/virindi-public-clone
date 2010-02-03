

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Tue Feb 02 20:03:04 2010
 */
/* Compiler settings for .\RenderHook.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __RenderHook_h__
#define __RenderHook_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ISVRenderHook_FWD_DEFINED__
#define __ISVRenderHook_FWD_DEFINED__
typedef interface ISVRenderHook ISVRenderHook;
#endif 	/* __ISVRenderHook_FWD_DEFINED__ */


#ifndef __SVRenderHook_FWD_DEFINED__
#define __SVRenderHook_FWD_DEFINED__

#ifdef __cplusplus
typedef class SVRenderHook SVRenderHook;
#else
typedef struct SVRenderHook SVRenderHook;
#endif /* __cplusplus */

#endif 	/* __SVRenderHook_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

#ifndef __ISVRenderHook_INTERFACE_DEFINED__
#define __ISVRenderHook_INTERFACE_DEFINED__

/* interface ISVRenderHook */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ISVRenderHook;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F5E367AA-6FC9-473B-8BC8-9060C25EFA39")
    ISVRenderHook : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_fEnabled( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_fEnabled( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_fSlope( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_fSlope( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_fTrace( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_fTrace( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TraceOneFrame( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ IUnknown *punkNetSvc) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_fWater( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_fWater( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_colorSlope( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_colorSlope( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_colorWater( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_colorWater( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Finalize( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_fLight( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_fLight( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_colorLight( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_colorLight( 
            /* [in] */ LONG newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ISVRenderHookVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ISVRenderHook * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ISVRenderHook * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ISVRenderHook * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ISVRenderHook * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ISVRenderHook * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ISVRenderHook * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ISVRenderHook * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_fEnabled )( 
            ISVRenderHook * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_fEnabled )( 
            ISVRenderHook * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_fSlope )( 
            ISVRenderHook * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_fSlope )( 
            ISVRenderHook * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_fTrace )( 
            ISVRenderHook * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_fTrace )( 
            ISVRenderHook * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *TraceOneFrame )( 
            ISVRenderHook * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            ISVRenderHook * This,
            /* [in] */ IUnknown *punkNetSvc);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_fWater )( 
            ISVRenderHook * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_fWater )( 
            ISVRenderHook * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_colorSlope )( 
            ISVRenderHook * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_colorSlope )( 
            ISVRenderHook * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_colorWater )( 
            ISVRenderHook * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_colorWater )( 
            ISVRenderHook * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Finalize )( 
            ISVRenderHook * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_fLight )( 
            ISVRenderHook * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_fLight )( 
            ISVRenderHook * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_colorLight )( 
            ISVRenderHook * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_colorLight )( 
            ISVRenderHook * This,
            /* [in] */ LONG newVal);
        
        END_INTERFACE
    } ISVRenderHookVtbl;

    interface ISVRenderHook
    {
        CONST_VTBL struct ISVRenderHookVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISVRenderHook_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ISVRenderHook_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ISVRenderHook_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ISVRenderHook_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define ISVRenderHook_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define ISVRenderHook_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define ISVRenderHook_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define ISVRenderHook_get_fEnabled(This,pVal)	\
    (This)->lpVtbl -> get_fEnabled(This,pVal)

#define ISVRenderHook_put_fEnabled(This,newVal)	\
    (This)->lpVtbl -> put_fEnabled(This,newVal)

#define ISVRenderHook_get_fSlope(This,pVal)	\
    (This)->lpVtbl -> get_fSlope(This,pVal)

#define ISVRenderHook_put_fSlope(This,newVal)	\
    (This)->lpVtbl -> put_fSlope(This,newVal)

#define ISVRenderHook_get_fTrace(This,pVal)	\
    (This)->lpVtbl -> get_fTrace(This,pVal)

#define ISVRenderHook_put_fTrace(This,newVal)	\
    (This)->lpVtbl -> put_fTrace(This,newVal)

#define ISVRenderHook_TraceOneFrame(This)	\
    (This)->lpVtbl -> TraceOneFrame(This)

#define ISVRenderHook_Init(This,punkNetSvc)	\
    (This)->lpVtbl -> Init(This,punkNetSvc)

#define ISVRenderHook_get_fWater(This,pVal)	\
    (This)->lpVtbl -> get_fWater(This,pVal)

#define ISVRenderHook_put_fWater(This,newVal)	\
    (This)->lpVtbl -> put_fWater(This,newVal)

#define ISVRenderHook_get_colorSlope(This,pVal)	\
    (This)->lpVtbl -> get_colorSlope(This,pVal)

#define ISVRenderHook_put_colorSlope(This,newVal)	\
    (This)->lpVtbl -> put_colorSlope(This,newVal)

#define ISVRenderHook_get_colorWater(This,pVal)	\
    (This)->lpVtbl -> get_colorWater(This,pVal)

#define ISVRenderHook_put_colorWater(This,newVal)	\
    (This)->lpVtbl -> put_colorWater(This,newVal)

#define ISVRenderHook_Finalize(This)	\
    (This)->lpVtbl -> Finalize(This)

#define ISVRenderHook_get_fLight(This,pVal)	\
    (This)->lpVtbl -> get_fLight(This,pVal)

#define ISVRenderHook_put_fLight(This,newVal)	\
    (This)->lpVtbl -> put_fLight(This,newVal)

#define ISVRenderHook_get_colorLight(This,pVal)	\
    (This)->lpVtbl -> get_colorLight(This,pVal)

#define ISVRenderHook_put_colorLight(This,newVal)	\
    (This)->lpVtbl -> put_colorLight(This,newVal)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_fEnabled_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ VARIANT_BOOL *pVal);


void __RPC_STUB ISVRenderHook_get_fEnabled_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_fEnabled_Proxy( 
    ISVRenderHook * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ISVRenderHook_put_fEnabled_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_fSlope_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ VARIANT_BOOL *pVal);


void __RPC_STUB ISVRenderHook_get_fSlope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_fSlope_Proxy( 
    ISVRenderHook * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ISVRenderHook_put_fSlope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_fTrace_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ VARIANT_BOOL *pVal);


void __RPC_STUB ISVRenderHook_get_fTrace_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_fTrace_Proxy( 
    ISVRenderHook * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ISVRenderHook_put_fTrace_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_TraceOneFrame_Proxy( 
    ISVRenderHook * This);


void __RPC_STUB ISVRenderHook_TraceOneFrame_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_Init_Proxy( 
    ISVRenderHook * This,
    /* [in] */ IUnknown *punkNetSvc);


void __RPC_STUB ISVRenderHook_Init_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_fWater_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ VARIANT_BOOL *pVal);


void __RPC_STUB ISVRenderHook_get_fWater_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_fWater_Proxy( 
    ISVRenderHook * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ISVRenderHook_put_fWater_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_colorSlope_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ LONG *pVal);


void __RPC_STUB ISVRenderHook_get_colorSlope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_colorSlope_Proxy( 
    ISVRenderHook * This,
    /* [in] */ LONG newVal);


void __RPC_STUB ISVRenderHook_put_colorSlope_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_colorWater_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ LONG *pVal);


void __RPC_STUB ISVRenderHook_get_colorWater_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_colorWater_Proxy( 
    ISVRenderHook * This,
    /* [in] */ LONG newVal);


void __RPC_STUB ISVRenderHook_put_colorWater_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_Finalize_Proxy( 
    ISVRenderHook * This);


void __RPC_STUB ISVRenderHook_Finalize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_fLight_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ VARIANT_BOOL *pVal);


void __RPC_STUB ISVRenderHook_get_fLight_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_fLight_Proxy( 
    ISVRenderHook * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ISVRenderHook_put_fLight_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_get_colorLight_Proxy( 
    ISVRenderHook * This,
    /* [retval][out] */ LONG *pVal);


void __RPC_STUB ISVRenderHook_get_colorLight_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ISVRenderHook_put_colorLight_Proxy( 
    ISVRenderHook * This,
    /* [in] */ LONG newVal);


void __RPC_STUB ISVRenderHook_put_colorLight_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ISVRenderHook_INTERFACE_DEFINED__ */



#ifndef __RenderHookLib_LIBRARY_DEFINED__
#define __RenderHookLib_LIBRARY_DEFINED__

/* library RenderHookLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_RenderHookLib;

EXTERN_C const CLSID CLSID_SVRenderHook;

#ifdef __cplusplus

class DECLSPEC_UUID("084DB7D3-FCA8-4C37-8748-18232FE9CF9A")
SVRenderHook;
#endif
#endif /* __RenderHookLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


