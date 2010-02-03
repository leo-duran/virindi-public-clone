// HookAll.cpp

#include "stdafx.h"
#include "Trace.h"

LPVOID HookVTable(LPVOID lpObject, ULONG ulIndex, LPVOID lpOriginal, LPVOID lpDetour);
LPVOID UnhookVTable(LPVOID lpObject, ULONG ulIndex, LPVOID lpOriginal, LPVOID lpDetour);


// Original function pointers:

HRESULT (STDMETHODCALLTYPE *TestCooperativeLevelOrig)(IDirect3DDevice9* pD3DD9) = NULL;
UINT (STDMETHODCALLTYPE *GetAvailableTextureMemOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *EvictManagedResourcesOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *GetDirect3DOrig)(IDirect3DDevice9* pD3DD9, IDirect3D9** ppD3D9) = NULL;
HRESULT (STDMETHODCALLTYPE *GetDeviceCapsOrig)(IDirect3DDevice9* pD3DD9, D3DCAPS9* pCaps) = NULL;
HRESULT (STDMETHODCALLTYPE *GetDisplayModeOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DDISPLAYMODE* pMode) = NULL;
HRESULT (STDMETHODCALLTYPE *GetCreationParametersOrig)(IDirect3DDevice9* pD3DD9, D3DDEVICE_CREATION_PARAMETERS* pParameters) = NULL;
HRESULT (STDMETHODCALLTYPE *SetCursorPropertiesOrig)(IDirect3DDevice9* pD3DD9, UINT XHotSpot, UINT YHotSpot, IDirect3DSurface9* pCursorBitmap) = NULL;
void (STDMETHODCALLTYPE *SetCursorPositionOrig)(IDirect3DDevice9* pD3DD9, int X, int Y, DWORD Flags) = NULL;
BOOL (STDMETHODCALLTYPE *ShowCursorOrig)(IDirect3DDevice9* pD3DD9, BOOL bShow) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateAdditionalSwapChainOrig)(IDirect3DDevice9* pD3DD9, D3DPRESENT_PARAMETERS* pPresentationParameters, IDirect3DSwapChain9** pSwapChain) = NULL;
HRESULT (STDMETHODCALLTYPE *GetSwapChainOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, IDirect3DSwapChain9** pSwapChain) = NULL;
UINT (STDMETHODCALLTYPE *GetNumberOfSwapChainsOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *ResetOrig)(IDirect3DDevice9* pD3DD9, D3DPRESENT_PARAMETERS* pPresentationParameters) = NULL;
HRESULT (STDMETHODCALLTYPE *PresentOrig)(IDirect3DDevice9* pD3DD9, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion) = NULL;
HRESULT (STDMETHODCALLTYPE *GetBackBufferOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, UINT iBackBuffer, D3DBACKBUFFER_TYPE Type, IDirect3DSurface9** ppBackBuffer) = NULL;
HRESULT (STDMETHODCALLTYPE *GetRasterStatusOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DRASTER_STATUS* pRasterStatus) = NULL;
HRESULT (STDMETHODCALLTYPE *SetDialogBoxModeOrig)(IDirect3DDevice9* pD3DD9, BOOL bEnableDialogs) = NULL;
void (STDMETHODCALLTYPE *SetGammaRampOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, DWORD Flags, CONST D3DGAMMARAMP* pRamp) = NULL;
void (STDMETHODCALLTYPE *GetGammaRampOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DGAMMARAMP* pRamp) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateTextureOrig)(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DTexture9** ppTexture, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateVolumeTextureOrig)(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, UINT Depth, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DVolumeTexture9** ppVolumeTexture, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateCubeTextureOrig)(IDirect3DDevice9* pD3DD9, UINT EdgeLength, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DCubeTexture9** ppCubeTexture, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateVertexBufferOrig)(IDirect3DDevice9* pD3DD9, UINT Length, DWORD Usage, DWORD FVF, D3DPOOL Pool, IDirect3DVertexBuffer9** ppVertexBuffer, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateIndexBufferOrig)(IDirect3DDevice9* pD3DD9, UINT Length, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DIndexBuffer9** ppIndexBuffer, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateRenderTargetOrig)(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DMULTISAMPLE_TYPE MultiSample, DWORD MultisampleQuality, BOOL Lockable, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateDepthStencilSurfaceOrig)(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DMULTISAMPLE_TYPE MultiSample, DWORD MultisampleQuality, BOOL Discard, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *UpdateSurfaceOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSourceSurface, CONST RECT* pSourceRect, IDirect3DSurface9* pDestinationSurface, CONST POINT* pDestPoint) = NULL;
HRESULT (STDMETHODCALLTYPE *UpdateTextureOrig)(IDirect3DDevice9* pD3DD9, IDirect3DBaseTexture9* pSourceTexture, IDirect3DBaseTexture9* pDestinationTexture) = NULL;
HRESULT (STDMETHODCALLTYPE *GetRenderTargetDataOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pRenderTarget, IDirect3DSurface9* pDestSurface) = NULL;
HRESULT (STDMETHODCALLTYPE *GetFrontBufferDataOrig)(IDirect3DDevice9* pD3DD9, UINT iSwapChain, IDirect3DSurface9* pDestSurface) = NULL;
HRESULT (STDMETHODCALLTYPE *StretchRectOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSourceSurface, CONST RECT* pSourceRect, IDirect3DSurface9* pDestSurface, CONST RECT* pDestRect, D3DTEXTUREFILTERTYPE Filter) = NULL;
HRESULT (STDMETHODCALLTYPE *ColorFillOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSurface, CONST RECT* pRect, D3DCOLOR color) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateOffscreenPlainSurfaceOrig)(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DPOOL Pool, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle) = NULL;
HRESULT (STDMETHODCALLTYPE *SetRenderTargetOrig)(IDirect3DDevice9* pD3DD9, DWORD RenderTargetIndex, IDirect3DSurface9* pRenderTarget) = NULL;
HRESULT (STDMETHODCALLTYPE *GetRenderTargetOrig)(IDirect3DDevice9* pD3DD9, DWORD RenderTargetIndex, IDirect3DSurface9** ppRenderTarget) = NULL;
HRESULT (STDMETHODCALLTYPE *SetDepthStencilSurfaceOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pNewZStencil) = NULL;
HRESULT (STDMETHODCALLTYPE *GetDepthStencilSurfaceOrig)(IDirect3DDevice9* pD3DD9, IDirect3DSurface9** ppZStencilSurface) = NULL;
HRESULT (STDMETHODCALLTYPE *BeginSceneOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *EndSceneOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *ClearOrig)(IDirect3DDevice9* pD3DD9, DWORD Count, CONST D3DRECT* pRects, DWORD Flags, D3DCOLOR Color, float Z, DWORD Stencil) = NULL;
HRESULT (STDMETHODCALLTYPE *SetTransformOrig)(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State, CONST D3DMATRIX* pMatrix) = NULL;
HRESULT (STDMETHODCALLTYPE *GetTransformOrig)(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State, D3DMATRIX* pMatrix) = NULL;
HRESULT (STDMETHODCALLTYPE *MultiplyTransformOrig)(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE arg0, CONST D3DMATRIX* arg1) = NULL;
HRESULT (STDMETHODCALLTYPE *SetViewportOrig)(IDirect3DDevice9* pD3DD9, CONST D3DVIEWPORT9* pViewport) = NULL;
HRESULT (STDMETHODCALLTYPE *GetViewportOrig)(IDirect3DDevice9* pD3DD9, D3DVIEWPORT9* pViewport) = NULL;
HRESULT (STDMETHODCALLTYPE *SetMaterialOrig)(IDirect3DDevice9* pD3DD9, CONST D3DMATERIAL9* pMaterial) = NULL;
HRESULT (STDMETHODCALLTYPE *GetMaterialOrig)(IDirect3DDevice9* pD3DD9, D3DMATERIAL9* pMaterial) = NULL;
HRESULT (STDMETHODCALLTYPE *SetLightOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, CONST D3DLIGHT9* arg1) = NULL;
HRESULT (STDMETHODCALLTYPE *GetLightOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, D3DLIGHT9* arg1) = NULL;
HRESULT (STDMETHODCALLTYPE *LightEnableOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, BOOL Enable) = NULL;
HRESULT (STDMETHODCALLTYPE *GetLightEnableOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, BOOL* pEnable) = NULL;
HRESULT (STDMETHODCALLTYPE *SetClipPlaneOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, CONST float* pPlane) = NULL;
HRESULT (STDMETHODCALLTYPE *GetClipPlaneOrig)(IDirect3DDevice9* pD3DD9, DWORD Index, float* pPlane) = NULL;
HRESULT (STDMETHODCALLTYPE *SetRenderStateOrig)(IDirect3DDevice9* pD3DD9, D3DRENDERSTATETYPE State, DWORD Value) = NULL;
HRESULT (STDMETHODCALLTYPE *GetRenderStateOrig)(IDirect3DDevice9* pD3DD9, D3DRENDERSTATETYPE State, DWORD* pValue) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateStateBlockOrig)(IDirect3DDevice9* pD3DD9, D3DSTATEBLOCKTYPE Type, IDirect3DStateBlock9** ppSB) = NULL;
HRESULT (STDMETHODCALLTYPE *BeginStateBlockOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *EndStateBlockOrig)(IDirect3DDevice9* pD3DD9, IDirect3DStateBlock9** ppSB) = NULL;
HRESULT (STDMETHODCALLTYPE *SetClipStatusOrig)(IDirect3DDevice9* pD3DD9, CONST D3DCLIPSTATUS9* pClipStatus) = NULL;
HRESULT (STDMETHODCALLTYPE *GetClipStatusOrig)(IDirect3DDevice9* pD3DD9, D3DCLIPSTATUS9* pClipStatus) = NULL;
HRESULT (STDMETHODCALLTYPE *GetTextureOrig)(IDirect3DDevice9* pD3DD9, DWORD Stage, IDirect3DBaseTexture9** ppTexture) = NULL;
HRESULT (STDMETHODCALLTYPE *SetTextureOrig)(IDirect3DDevice9* pD3DD9, DWORD Stage, IDirect3DBaseTexture9* pTexture) = NULL;
HRESULT (STDMETHODCALLTYPE *GetTextureStageStateOrig)(IDirect3DDevice9* pD3DD9, DWORD Stage, D3DTEXTURESTAGESTATETYPE Type, DWORD* pValue) = NULL;
HRESULT (STDMETHODCALLTYPE *SetTextureStageStateOrig)(IDirect3DDevice9* pD3DD9, DWORD Stage, D3DTEXTURESTAGESTATETYPE Type, DWORD Value) = NULL;
HRESULT (STDMETHODCALLTYPE *GetSamplerStateOrig)(IDirect3DDevice9* pD3DD9, DWORD Sampler, D3DSAMPLERSTATETYPE Type, DWORD* pValue) = NULL;
HRESULT (STDMETHODCALLTYPE *SetSamplerStateOrig)(IDirect3DDevice9* pD3DD9, DWORD Sampler, D3DSAMPLERSTATETYPE Type, DWORD Value) = NULL;
HRESULT (STDMETHODCALLTYPE *ValidateDeviceOrig)(IDirect3DDevice9* pD3DD9, DWORD* pNumPasses) = NULL;
HRESULT (STDMETHODCALLTYPE *SetPaletteEntriesOrig)(IDirect3DDevice9* pD3DD9, UINT PaletteNumber, CONST PALETTEENTRY* pEntries) = NULL;
HRESULT (STDMETHODCALLTYPE *GetPaletteEntriesOrig)(IDirect3DDevice9* pD3DD9, UINT PaletteNumber, PALETTEENTRY* pEntries) = NULL;
HRESULT (STDMETHODCALLTYPE *SetCurrentTexturePaletteOrig)(IDirect3DDevice9* pD3DD9, UINT PaletteNumber) = NULL;
HRESULT (STDMETHODCALLTYPE *GetCurrentTexturePaletteOrig)(IDirect3DDevice9* pD3DD9, UINT* PaletteNumber) = NULL;
HRESULT (STDMETHODCALLTYPE *SetScissorRectOrig)(IDirect3DDevice9* pD3DD9, CONST RECT* pRect) = NULL;
HRESULT (STDMETHODCALLTYPE *GetScissorRectOrig)(IDirect3DDevice9* pD3DD9, RECT* pRect) = NULL;
HRESULT (STDMETHODCALLTYPE *SetSoftwareVertexProcessingOrig)(IDirect3DDevice9* pD3DD9, BOOL bSoftware) = NULL;
BOOL (STDMETHODCALLTYPE *GetSoftwareVertexProcessingOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *SetNPatchModeOrig)(IDirect3DDevice9* pD3DD9, float nSegments) = NULL;
float (STDMETHODCALLTYPE *GetNPatchModeOrig)(IDirect3DDevice9* pD3DD9) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawPrimitiveOrig)(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT StartVertex, UINT PrimitiveCount) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawIndexedPrimitiveOrig)(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE arg0, INT BaseVertexIndex, UINT MinVertexIndex, UINT NumVertices, UINT startIndex, UINT primCount) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawPrimitiveUPOrig)(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT PrimitiveCount, CONST void* pVertexStreamZeroData, UINT VertexStreamZeroStride) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawIndexedPrimitiveUPOrig)(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT MinVertexIndex, UINT NumVertices, UINT PrimitiveCount, CONST void* pIndexData, D3DFORMAT IndexDataFormat, CONST void* pVertexStreamZeroData, UINT VertexStreamZeroStride) = NULL;
HRESULT (STDMETHODCALLTYPE *ProcessVerticesOrig)(IDirect3DDevice9* pD3DD9, UINT SrcStartIndex, UINT DestIndex, UINT VertexCount, IDirect3DVertexBuffer9* pDestBuffer, IDirect3DVertexDeclaration9* pVertexDecl, DWORD Flags) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateVertexDeclarationOrig)(IDirect3DDevice9* pD3DD9, CONST D3DVERTEXELEMENT9* pVertexElements, IDirect3DVertexDeclaration9** ppDecl) = NULL;
HRESULT (STDMETHODCALLTYPE *SetVertexDeclarationOrig)(IDirect3DDevice9* pD3DD9, IDirect3DVertexDeclaration9* pDecl) = NULL;
HRESULT (STDMETHODCALLTYPE *GetVertexDeclarationOrig)(IDirect3DDevice9* pD3DD9, IDirect3DVertexDeclaration9** ppDecl) = NULL;
HRESULT (STDMETHODCALLTYPE *SetFVFOrig)(IDirect3DDevice9* pD3DD9, DWORD FVF) = NULL;
HRESULT (STDMETHODCALLTYPE *GetFVFOrig)(IDirect3DDevice9* pD3DD9, DWORD* pFVF) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateVertexShaderOrig)(IDirect3DDevice9* pD3DD9, CONST DWORD* pFunction, IDirect3DVertexShader9** ppShader) = NULL;
HRESULT (STDMETHODCALLTYPE *SetVertexShaderOrig)(IDirect3DDevice9* pD3DD9, IDirect3DVertexShader9* pShader) = NULL;
HRESULT (STDMETHODCALLTYPE *GetVertexShaderOrig)(IDirect3DDevice9* pD3DD9, IDirect3DVertexShader9** ppShader) = NULL;
HRESULT (STDMETHODCALLTYPE *SetVertexShaderConstantFOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST float* pConstantData, UINT Vector4fCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetVertexShaderConstantFOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, float* pConstantData, UINT Vector4fCount) = NULL;
HRESULT (STDMETHODCALLTYPE *SetVertexShaderConstantIOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST int* pConstantData, UINT Vector4iCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetVertexShaderConstantIOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, int* pConstantData, UINT Vector4iCount) = NULL;
HRESULT (STDMETHODCALLTYPE *SetVertexShaderConstantBOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST BOOL* pConstantData, UINT BoolCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetVertexShaderConstantBOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, BOOL* pConstantData, UINT BoolCount) = NULL;
HRESULT (STDMETHODCALLTYPE *SetStreamSourceOrig)(IDirect3DDevice9* pD3DD9, UINT StreamNumber, IDirect3DVertexBuffer9* pStreamData, UINT OffsetInBytes, UINT Stride) = NULL;
HRESULT (STDMETHODCALLTYPE *GetStreamSourceOrig)(IDirect3DDevice9* pD3DD9, UINT StreamNumber, IDirect3DVertexBuffer9** ppStreamData, UINT* pOffsetInBytes, UINT* pStride) = NULL;
HRESULT (STDMETHODCALLTYPE *SetStreamSourceFreqOrig)(IDirect3DDevice9* pD3DD9, UINT StreamNumber, UINT Setting) = NULL;
HRESULT (STDMETHODCALLTYPE *GetStreamSourceFreqOrig)(IDirect3DDevice9* pD3DD9, UINT StreamNumber, UINT* pSetting) = NULL;
HRESULT (STDMETHODCALLTYPE *SetIndicesOrig)(IDirect3DDevice9* pD3DD9, IDirect3DIndexBuffer9* pIndexData) = NULL;
HRESULT (STDMETHODCALLTYPE *GetIndicesOrig)(IDirect3DDevice9* pD3DD9, IDirect3DIndexBuffer9** ppIndexData) = NULL;
HRESULT (STDMETHODCALLTYPE *CreatePixelShaderOrig)(IDirect3DDevice9* pD3DD9, CONST DWORD* pFunction, IDirect3DPixelShader9** ppShader) = NULL;
HRESULT (STDMETHODCALLTYPE *SetPixelShaderOrig)(IDirect3DDevice9* pD3DD9, IDirect3DPixelShader9* pShader) = NULL;
HRESULT (STDMETHODCALLTYPE *GetPixelShaderOrig)(IDirect3DDevice9* pD3DD9, IDirect3DPixelShader9** ppShader) = NULL;
HRESULT (STDMETHODCALLTYPE *SetPixelShaderConstantFOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST float* pConstantData, UINT Vector4fCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetPixelShaderConstantFOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, float* pConstantData, UINT Vector4fCount) = NULL;
HRESULT (STDMETHODCALLTYPE *SetPixelShaderConstantIOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST int* pConstantData, UINT Vector4iCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetPixelShaderConstantIOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, int* pConstantData, UINT Vector4iCount) = NULL;
HRESULT (STDMETHODCALLTYPE *SetPixelShaderConstantBOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST BOOL* pConstantData, UINT BoolCount) = NULL;
HRESULT (STDMETHODCALLTYPE *GetPixelShaderConstantBOrig)(IDirect3DDevice9* pD3DD9, UINT StartRegister, BOOL* pConstantData, UINT BoolCount) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawRectPatchOrig)(IDirect3DDevice9* pD3DD9, UINT Handle, CONST float* pNumSegs, CONST D3DRECTPATCH_INFO* pRectPatchInfo) = NULL;
HRESULT (STDMETHODCALLTYPE *DrawTriPatchOrig)(IDirect3DDevice9* pD3DD9, UINT Handle, CONST float* pNumSegs, CONST D3DTRIPATCH_INFO* pTriPatchInfo) = NULL;
HRESULT (STDMETHODCALLTYPE *DeletePatchOrig)(IDirect3DDevice9* pD3DD9, UINT Handle) = NULL;
HRESULT (STDMETHODCALLTYPE *CreateQueryOrig)(IDirect3DDevice9* pD3DD9, D3DQUERYTYPE Type, IDirect3DQuery9** ppQuery) = NULL;


// Hook functions:

HRESULT STDMETHODCALLTYPE TestCooperativeLevelHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("TestCooperativeLevel()"));
    return TestCooperativeLevelOrig(pD3DD9);
    }

UINT STDMETHODCALLTYPE GetAvailableTextureMemHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("GetAvailableTextureMem()"));
    return GetAvailableTextureMemOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE EvictManagedResourcesHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("EvictManagedResources()"));
    return EvictManagedResourcesOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE GetDirect3DHook(IDirect3DDevice9* pD3DD9, IDirect3D9** ppD3D9)
    {
    DebugLine(_T("GetDirect3D(%08X)"), ppD3D9);
    return GetDirect3DOrig(pD3DD9, ppD3D9);
    }

HRESULT STDMETHODCALLTYPE GetDeviceCapsHook(IDirect3DDevice9* pD3DD9, D3DCAPS9* pCaps)
    {
    DebugLine(_T("GetDeviceCaps(%08X)"), pCaps);
    return GetDeviceCapsOrig(pD3DD9, pCaps);
    }

HRESULT STDMETHODCALLTYPE GetDisplayModeHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DDISPLAYMODE* pMode)
    {
    DebugLine(_T("GetDisplayMode(%08X, %08X)"), iSwapChain, pMode);
    return GetDisplayModeOrig(pD3DD9, iSwapChain, pMode);
    }

HRESULT STDMETHODCALLTYPE GetCreationParametersHook(IDirect3DDevice9* pD3DD9, D3DDEVICE_CREATION_PARAMETERS* pParameters)
    {
    DebugLine(_T("GetCreationParameters(%08X)"), pParameters);
    return GetCreationParametersOrig(pD3DD9, pParameters);
    }

HRESULT STDMETHODCALLTYPE SetCursorPropertiesHook(IDirect3DDevice9* pD3DD9, UINT XHotSpot, UINT YHotSpot, IDirect3DSurface9* pCursorBitmap)
    {
    DebugLine(_T("SetCursorProperties(%08X, %08X, %08X)"), XHotSpot, YHotSpot, pCursorBitmap);
    return SetCursorPropertiesOrig(pD3DD9, XHotSpot, YHotSpot, pCursorBitmap);
    }

void STDMETHODCALLTYPE SetCursorPositionHook(IDirect3DDevice9* pD3DD9, int X, int Y, DWORD Flags)
    {
    DebugLine(_T("SetCursorPosition(%08X, %08X, %08X)"), X, Y, Flags);
    return SetCursorPositionOrig(pD3DD9, X, Y, Flags);
    }

BOOL STDMETHODCALLTYPE ShowCursorHook(IDirect3DDevice9* pD3DD9, BOOL bShow)
    {
    DebugLine(_T("ShowCursor(%08X)"), bShow);
    return ShowCursorOrig(pD3DD9, bShow);
    }

HRESULT STDMETHODCALLTYPE CreateAdditionalSwapChainHook(IDirect3DDevice9* pD3DD9, D3DPRESENT_PARAMETERS* pPresentationParameters, IDirect3DSwapChain9** pSwapChain)
    {
    DebugLine(_T("CreateAdditionalSwapChain(%08X, %08X)"), pPresentationParameters, pSwapChain);
    return CreateAdditionalSwapChainOrig(pD3DD9, pPresentationParameters, pSwapChain);
    }

HRESULT STDMETHODCALLTYPE GetSwapChainHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, IDirect3DSwapChain9** pSwapChain)
    {
    DebugLine(_T("GetSwapChain(%08X, %08X)"), iSwapChain, pSwapChain);
    return GetSwapChainOrig(pD3DD9, iSwapChain, pSwapChain);
    }

UINT STDMETHODCALLTYPE GetNumberOfSwapChainsHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("GetNumberOfSwapChains()"));
    return GetNumberOfSwapChainsOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE ResetHook(IDirect3DDevice9* pD3DD9, D3DPRESENT_PARAMETERS* pPresentationParameters)
    {
    DebugLine(_T("Reset(%08X)"), pPresentationParameters);
    return ResetOrig(pD3DD9, pPresentationParameters);
    }

HRESULT STDMETHODCALLTYPE PresentHook(IDirect3DDevice9* pD3DD9, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion)
    {
    DebugLine(_T("Present(%08X, %08X, %08X, %08X)"), pSourceRect, pDestRect, hDestWindowOverride, pDirtyRegion);
    return PresentOrig(pD3DD9, pSourceRect, pDestRect, hDestWindowOverride, pDirtyRegion);
    }

HRESULT STDMETHODCALLTYPE GetBackBufferHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, UINT iBackBuffer, D3DBACKBUFFER_TYPE Type, IDirect3DSurface9** ppBackBuffer)
    {
    DebugLine(_T("GetBackBuffer(%08X, %08X, %08X, %08X)"), iSwapChain, iBackBuffer, Type, ppBackBuffer);
    return GetBackBufferOrig(pD3DD9, iSwapChain, iBackBuffer, Type, ppBackBuffer);
    }

HRESULT STDMETHODCALLTYPE GetRasterStatusHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DRASTER_STATUS* pRasterStatus)
    {
    DebugLine(_T("GetRasterStatus(%08X, %08X)"), iSwapChain, pRasterStatus);
    return GetRasterStatusOrig(pD3DD9, iSwapChain, pRasterStatus);
    }

HRESULT STDMETHODCALLTYPE SetDialogBoxModeHook(IDirect3DDevice9* pD3DD9, BOOL bEnableDialogs)
    {
    DebugLine(_T("SetDialogBoxMode(%08X)"), bEnableDialogs);
    return SetDialogBoxModeOrig(pD3DD9, bEnableDialogs);
    }

void STDMETHODCALLTYPE SetGammaRampHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, DWORD Flags, CONST D3DGAMMARAMP* pRamp)
    {
    DebugLine(_T("SetGammaRamp(%08X, %08X, %08X)"), iSwapChain, Flags, pRamp);
    return SetGammaRampOrig(pD3DD9, iSwapChain, Flags, pRamp);
    }

void STDMETHODCALLTYPE GetGammaRampHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, D3DGAMMARAMP* pRamp)
    {
    DebugLine(_T("GetGammaRamp(%08X, %08X)"), iSwapChain, pRamp);
    return GetGammaRampOrig(pD3DD9, iSwapChain, pRamp);
    }

HRESULT STDMETHODCALLTYPE CreateTextureHook(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DTexture9** ppTexture, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateTexture(%08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X)"), Width, Height, Levels, Usage, Format, Pool, ppTexture, pSharedHandle);
    return CreateTextureOrig(pD3DD9, Width, Height, Levels, Usage, Format, Pool, ppTexture, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateVolumeTextureHook(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, UINT Depth, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DVolumeTexture9** ppVolumeTexture, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateVolumeTexture(%08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X)"), Width, Height, Depth, Levels, Usage, Format, Pool, ppVolumeTexture, pSharedHandle);
    return CreateVolumeTextureOrig(pD3DD9, Width, Height, Depth, Levels, Usage, Format, Pool, ppVolumeTexture, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateCubeTextureHook(IDirect3DDevice9* pD3DD9, UINT EdgeLength, UINT Levels, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DCubeTexture9** ppCubeTexture, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateCubeTexture(%08X, %08X, %08X, %08X, %08X, %08X, %08X)"), EdgeLength, Levels, Usage, Format, Pool, ppCubeTexture, pSharedHandle);
    return CreateCubeTextureOrig(pD3DD9, EdgeLength, Levels, Usage, Format, Pool, ppCubeTexture, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateVertexBufferHook(IDirect3DDevice9* pD3DD9, UINT Length, DWORD Usage, DWORD FVF, D3DPOOL Pool, IDirect3DVertexBuffer9** ppVertexBuffer, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateVertexBuffer(%08X, %08X, %08X, %08X, %08X, %08X)"), Length, Usage, FVF, Pool, ppVertexBuffer, pSharedHandle);
    return CreateVertexBufferOrig(pD3DD9, Length, Usage, FVF, Pool, ppVertexBuffer, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateIndexBufferHook(IDirect3DDevice9* pD3DD9, UINT Length, DWORD Usage, D3DFORMAT Format, D3DPOOL Pool, IDirect3DIndexBuffer9** ppIndexBuffer, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateIndexBuffer(%08X, %08X, %08X, %08X, %08X, %08X)"), Length, Usage, Format, Pool, ppIndexBuffer, pSharedHandle);
    return CreateIndexBufferOrig(pD3DD9, Length, Usage, Format, Pool, ppIndexBuffer, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateRenderTargetHook(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DMULTISAMPLE_TYPE MultiSample, DWORD MultisampleQuality, BOOL Lockable, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateRenderTarget(%08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X)"), Width, Height, Format, MultiSample, MultisampleQuality, Lockable, ppSurface, pSharedHandle);
    return CreateRenderTargetOrig(pD3DD9, Width, Height, Format, MultiSample, MultisampleQuality, Lockable, ppSurface, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE CreateDepthStencilSurfaceHook(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DMULTISAMPLE_TYPE MultiSample, DWORD MultisampleQuality, BOOL Discard, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateDepthStencilSurface(%08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X)"), Width, Height, Format, MultiSample, MultisampleQuality, Discard, ppSurface, pSharedHandle);
    return CreateDepthStencilSurfaceOrig(pD3DD9, Width, Height, Format, MultiSample, MultisampleQuality, Discard, ppSurface, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE UpdateSurfaceHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSourceSurface, CONST RECT* pSourceRect, IDirect3DSurface9* pDestinationSurface, CONST POINT* pDestPoint)
    {
    DebugLine(_T("UpdateSurface(%08X, %08X, %08X, %08X)"), pSourceSurface, pSourceRect, pDestinationSurface, pDestPoint);
    return UpdateSurfaceOrig(pD3DD9, pSourceSurface, pSourceRect, pDestinationSurface, pDestPoint);
    }

HRESULT STDMETHODCALLTYPE UpdateTextureHook(IDirect3DDevice9* pD3DD9, IDirect3DBaseTexture9* pSourceTexture, IDirect3DBaseTexture9* pDestinationTexture)
    {
    DebugLine(_T("UpdateTexture(%08X, %08X)"), pSourceTexture, pDestinationTexture);
    return UpdateTextureOrig(pD3DD9, pSourceTexture, pDestinationTexture);
    }

HRESULT STDMETHODCALLTYPE GetRenderTargetDataHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pRenderTarget, IDirect3DSurface9* pDestSurface)
    {
    DebugLine(_T("GetRenderTargetData(%08X, %08X)"), pRenderTarget, pDestSurface);
    return GetRenderTargetDataOrig(pD3DD9, pRenderTarget, pDestSurface);
    }

HRESULT STDMETHODCALLTYPE GetFrontBufferDataHook(IDirect3DDevice9* pD3DD9, UINT iSwapChain, IDirect3DSurface9* pDestSurface)
    {
    DebugLine(_T("GetFrontBufferData(%08X, %08X)"), iSwapChain, pDestSurface);
    return GetFrontBufferDataOrig(pD3DD9, iSwapChain, pDestSurface);
    }

HRESULT STDMETHODCALLTYPE StretchRectHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSourceSurface, CONST RECT* pSourceRect, IDirect3DSurface9* pDestSurface, CONST RECT* pDestRect, D3DTEXTUREFILTERTYPE Filter)
    {
    DebugLine(_T("StretchRect(%08X, %08X, %08X, %08X, %08X)"), pSourceSurface, pSourceRect, pDestSurface, pDestRect, Filter);
    return StretchRectOrig(pD3DD9, pSourceSurface, pSourceRect, pDestSurface, pDestRect, Filter);
    }

HRESULT STDMETHODCALLTYPE ColorFillHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pSurface, CONST RECT* pRect, D3DCOLOR color)
    {
    DebugLine(_T("ColorFill(%08X, %08X, %08X)"), pSurface, pRect, color);
    return ColorFillOrig(pD3DD9, pSurface, pRect, color);
    }

HRESULT STDMETHODCALLTYPE CreateOffscreenPlainSurfaceHook(IDirect3DDevice9* pD3DD9, UINT Width, UINT Height, D3DFORMAT Format, D3DPOOL Pool, IDirect3DSurface9** ppSurface, HANDLE* pSharedHandle)
    {
    DebugLine(_T("CreateOffscreenPlainSurface(%08X, %08X, %08X, %08X, %08X, %08X)"), Width, Height, Format, Pool, ppSurface, pSharedHandle);
    return CreateOffscreenPlainSurfaceOrig(pD3DD9, Width, Height, Format, Pool, ppSurface, pSharedHandle);
    }

HRESULT STDMETHODCALLTYPE SetRenderTargetHook(IDirect3DDevice9* pD3DD9, DWORD RenderTargetIndex, IDirect3DSurface9* pRenderTarget)
    {
    DebugLine(_T("SetRenderTarget(%08X, %08X)"), RenderTargetIndex, pRenderTarget);
    return SetRenderTargetOrig(pD3DD9, RenderTargetIndex, pRenderTarget);
    }

HRESULT STDMETHODCALLTYPE GetRenderTargetHook(IDirect3DDevice9* pD3DD9, DWORD RenderTargetIndex, IDirect3DSurface9** ppRenderTarget)
    {
    DebugLine(_T("GetRenderTarget(%08X, %08X)"), RenderTargetIndex, ppRenderTarget);
    return GetRenderTargetOrig(pD3DD9, RenderTargetIndex, ppRenderTarget);
    }

HRESULT STDMETHODCALLTYPE SetDepthStencilSurfaceHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9* pNewZStencil)
    {
    DebugLine(_T("SetDepthStencilSurface(%08X)"), pNewZStencil);
    return SetDepthStencilSurfaceOrig(pD3DD9, pNewZStencil);
    }

HRESULT STDMETHODCALLTYPE GetDepthStencilSurfaceHook(IDirect3DDevice9* pD3DD9, IDirect3DSurface9** ppZStencilSurface)
    {
    DebugLine(_T("GetDepthStencilSurface(%08X)"), ppZStencilSurface);
    return GetDepthStencilSurfaceOrig(pD3DD9, ppZStencilSurface);
    }

HRESULT STDMETHODCALLTYPE BeginSceneHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("BeginScene()"));
    return BeginSceneOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE EndSceneHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("EndScene()"));
    return EndSceneOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE ClearHook(IDirect3DDevice9* pD3DD9, DWORD Count, CONST D3DRECT* pRects, DWORD Flags, D3DCOLOR Color, float Z, DWORD Stencil)
    {
    DebugLine(_T("Clear(%08X, %08X, %08X, %08X, %0.2f, %08X)"), Count, pRects, Flags, Color, (double)Z, Stencil);
    return ClearOrig(pD3DD9, Count, pRects, Flags, Color, Z, Stencil);
    }

HRESULT STDMETHODCALLTYPE SetTransformHook(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State, CONST D3DMATRIX* pMatrix)
    {
    DebugLine(_T("SetTransform(%08X, %s)"), State, PszTraceMatrix(pMatrix));
    return SetTransformOrig(pD3DD9, State, pMatrix);
    }

HRESULT STDMETHODCALLTYPE GetTransformHook(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE State, D3DMATRIX* pMatrix)
    {
    DebugLine(_T("GetTransform(%08X, %08X)"), State, pMatrix);
    return GetTransformOrig(pD3DD9, State, pMatrix);
    }

HRESULT STDMETHODCALLTYPE MultiplyTransformHook(IDirect3DDevice9* pD3DD9, D3DTRANSFORMSTATETYPE arg0, CONST D3DMATRIX* arg1)
    {
    DebugLine(_T("MultiplyTransform(%08X, %s)"), arg0, PszTraceMatrix(arg1));
    return MultiplyTransformOrig(pD3DD9, arg0, arg1);
    }

HRESULT STDMETHODCALLTYPE SetViewportHook(IDirect3DDevice9* pD3DD9, CONST D3DVIEWPORT9* pViewport)
    {
    DebugLine(_T("SetViewport(%s)"), PszTraceViewport(pViewport));
    return SetViewportOrig(pD3DD9, pViewport);
    }

HRESULT STDMETHODCALLTYPE GetViewportHook(IDirect3DDevice9* pD3DD9, D3DVIEWPORT9* pViewport)
    {
    DebugLine(_T("GetViewport(%08X)"), pViewport);
    return GetViewportOrig(pD3DD9, pViewport);
    }

HRESULT STDMETHODCALLTYPE SetMaterialHook(IDirect3DDevice9* pD3DD9, CONST D3DMATERIAL9* pMaterial)
    {
    DebugLine(_T("SetMaterial(%s)"), PszTraceMaterial(pMaterial));
    return SetMaterialOrig(pD3DD9, pMaterial);
    }

HRESULT STDMETHODCALLTYPE GetMaterialHook(IDirect3DDevice9* pD3DD9, D3DMATERIAL9* pMaterial)
    {
    DebugLine(_T("GetMaterial(%08X)"), pMaterial);
    return GetMaterialOrig(pD3DD9, pMaterial);
    }

HRESULT STDMETHODCALLTYPE SetLightHook(IDirect3DDevice9* pD3DD9, DWORD Index, CONST D3DLIGHT9* arg1)
    {
    DebugLine(_T("SetLight(%08X, %s)"), Index, PszTraceLight(arg1));
    return SetLightOrig(pD3DD9, Index, arg1);
    }

HRESULT STDMETHODCALLTYPE GetLightHook(IDirect3DDevice9* pD3DD9, DWORD Index, D3DLIGHT9* arg1)
    {
    DebugLine(_T("GetLight(%08X, %08X)"), Index, arg1);
    return GetLightOrig(pD3DD9, Index, arg1);
    }

HRESULT STDMETHODCALLTYPE LightEnableHook(IDirect3DDevice9* pD3DD9, DWORD Index, BOOL Enable)
    {
    DebugLine(_T("LightEnable(%08X, %08X)"), Index, Enable);
    return LightEnableOrig(pD3DD9, Index, Enable);
    }

HRESULT STDMETHODCALLTYPE GetLightEnableHook(IDirect3DDevice9* pD3DD9, DWORD Index, BOOL* pEnable)
    {
    DebugLine(_T("GetLightEnable(%08X, %08X)"), Index, pEnable);
    return GetLightEnableOrig(pD3DD9, Index, pEnable);
    }

HRESULT STDMETHODCALLTYPE SetClipPlaneHook(IDirect3DDevice9* pD3DD9, DWORD Index, CONST float* pPlane)
    {
    DebugLine(_T("SetClipPlane(%08X, %08X)"), Index, pPlane);
    return SetClipPlaneOrig(pD3DD9, Index, pPlane);
    }

HRESULT STDMETHODCALLTYPE GetClipPlaneHook(IDirect3DDevice9* pD3DD9, DWORD Index, float* pPlane)
    {
    DebugLine(_T("GetClipPlane(%08X, %08X)"), Index, pPlane);
    return GetClipPlaneOrig(pD3DD9, Index, pPlane);
    }

HRESULT STDMETHODCALLTYPE SetRenderStateHook(IDirect3DDevice9* pD3DD9, D3DRENDERSTATETYPE State, DWORD Value)
    {
    DebugLine(_T("SetRenderState(%s, %08X)"), PszTraceRenderState(State), Value);
    return SetRenderStateOrig(pD3DD9, State, Value);
    }

HRESULT STDMETHODCALLTYPE GetRenderStateHook(IDirect3DDevice9* pD3DD9, D3DRENDERSTATETYPE State, DWORD* pValue)
    {
    DebugLine(_T("GetRenderState(%s, %08X)"), PszTraceRenderState(State), pValue);
    return GetRenderStateOrig(pD3DD9, State, pValue);
    }

HRESULT STDMETHODCALLTYPE CreateStateBlockHook(IDirect3DDevice9* pD3DD9, D3DSTATEBLOCKTYPE Type, IDirect3DStateBlock9** ppSB)
    {
    DebugLine(_T("CreateStateBlock(%08X, %08X)"), Type, ppSB);
    return CreateStateBlockOrig(pD3DD9, Type, ppSB);
    }

HRESULT STDMETHODCALLTYPE BeginStateBlockHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("BeginStateBlock()"));
    return BeginStateBlockOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE EndStateBlockHook(IDirect3DDevice9* pD3DD9, IDirect3DStateBlock9** ppSB)
    {
    DebugLine(_T("EndStateBlock(%08X)"), ppSB);
    return EndStateBlockOrig(pD3DD9, ppSB);
    }

HRESULT STDMETHODCALLTYPE SetClipStatusHook(IDirect3DDevice9* pD3DD9, CONST D3DCLIPSTATUS9* pClipStatus)
    {
    DebugLine(_T("SetClipStatus(%08X)"), pClipStatus);
    return SetClipStatusOrig(pD3DD9, pClipStatus);
    }

HRESULT STDMETHODCALLTYPE GetClipStatusHook(IDirect3DDevice9* pD3DD9, D3DCLIPSTATUS9* pClipStatus)
    {
    DebugLine(_T("GetClipStatus(%08X)"), pClipStatus);
    return GetClipStatusOrig(pD3DD9, pClipStatus);
    }

HRESULT STDMETHODCALLTYPE GetTextureHook(IDirect3DDevice9* pD3DD9, DWORD Stage, IDirect3DBaseTexture9** ppTexture)
    {
    DebugLine(_T("GetTexture(%08X, %08X)"), Stage, ppTexture);
    return GetTextureOrig(pD3DD9, Stage, ppTexture);
    }

HRESULT STDMETHODCALLTYPE SetTextureHook(IDirect3DDevice9* pD3DD9, DWORD Stage, IDirect3DBaseTexture9* pTexture)
    {
    DebugLine(_T("SetTexture(%08X, %08X)"), Stage, pTexture);
    return SetTextureOrig(pD3DD9, Stage, pTexture);
    }

HRESULT STDMETHODCALLTYPE GetTextureStageStateHook(IDirect3DDevice9* pD3DD9, DWORD Stage, D3DTEXTURESTAGESTATETYPE Type, DWORD* pValue)
    {
    DebugLine(_T("GetTextureStageState(%08X, %s, %08X)"), Stage, PszTraceTextureStageState(Type), pValue);
    return GetTextureStageStateOrig(pD3DD9, Stage, Type, pValue);
    }

HRESULT STDMETHODCALLTYPE SetTextureStageStateHook(IDirect3DDevice9* pD3DD9, DWORD Stage, D3DTEXTURESTAGESTATETYPE Type, DWORD Value)
    {
    DebugLine(_T("SetTextureStageState(%08X, %s, %08X)"), Stage, PszTraceTextureStageState(Type), Value);
    return SetTextureStageStateOrig(pD3DD9, Stage, Type, Value);
    }

HRESULT STDMETHODCALLTYPE GetSamplerStateHook(IDirect3DDevice9* pD3DD9, DWORD Sampler, D3DSAMPLERSTATETYPE Type, DWORD* pValue)
    {
    DebugLine(_T("GetSamplerState(%08X, %s, %08X)"), Sampler, PszTraceSamplerState(Type), pValue);
    return GetSamplerStateOrig(pD3DD9, Sampler, Type, pValue);
    }

HRESULT STDMETHODCALLTYPE SetSamplerStateHook(IDirect3DDevice9* pD3DD9, DWORD Sampler, D3DSAMPLERSTATETYPE Type, DWORD Value)
    {
    DebugLine(_T("SetSamplerState(%08X, %s, %08X)"), Sampler, PszTraceSamplerState(Type), Value);
    return SetSamplerStateOrig(pD3DD9, Sampler, Type, Value);
    }

HRESULT STDMETHODCALLTYPE ValidateDeviceHook(IDirect3DDevice9* pD3DD9, DWORD* pNumPasses)
    {
    DebugLine(_T("ValidateDevice(%08X)"), pNumPasses);
    return ValidateDeviceOrig(pD3DD9, pNumPasses);
    }

HRESULT STDMETHODCALLTYPE SetPaletteEntriesHook(IDirect3DDevice9* pD3DD9, UINT PaletteNumber, CONST PALETTEENTRY* pEntries)
    {
    DebugLine(_T("SetPaletteEntries(%08X, %08X)"), PaletteNumber, pEntries);
    return SetPaletteEntriesOrig(pD3DD9, PaletteNumber, pEntries);
    }

HRESULT STDMETHODCALLTYPE GetPaletteEntriesHook(IDirect3DDevice9* pD3DD9, UINT PaletteNumber, PALETTEENTRY* pEntries)
    {
    DebugLine(_T("GetPaletteEntries(%08X, %08X)"), PaletteNumber, pEntries);
    return GetPaletteEntriesOrig(pD3DD9, PaletteNumber, pEntries);
    }

HRESULT STDMETHODCALLTYPE SetCurrentTexturePaletteHook(IDirect3DDevice9* pD3DD9, UINT PaletteNumber)
    {
    DebugLine(_T("SetCurrentTexturePalette(%08X)"), PaletteNumber);
    return SetCurrentTexturePaletteOrig(pD3DD9, PaletteNumber);
    }

HRESULT STDMETHODCALLTYPE GetCurrentTexturePaletteHook(IDirect3DDevice9* pD3DD9, UINT* PaletteNumber)
    {
    DebugLine(_T("GetCurrentTexturePalette(%08X)"), PaletteNumber);
    return GetCurrentTexturePaletteOrig(pD3DD9, PaletteNumber);
    }

HRESULT STDMETHODCALLTYPE SetScissorRectHook(IDirect3DDevice9* pD3DD9, CONST RECT* pRect)
    {
    DebugLine(_T("SetScissorRect(%08X)"), pRect);
    return SetScissorRectOrig(pD3DD9, pRect);
    }

HRESULT STDMETHODCALLTYPE GetScissorRectHook(IDirect3DDevice9* pD3DD9, RECT* pRect)
    {
    DebugLine(_T("GetScissorRect(%08X)"), pRect);
    return GetScissorRectOrig(pD3DD9, pRect);
    }

HRESULT STDMETHODCALLTYPE SetSoftwareVertexProcessingHook(IDirect3DDevice9* pD3DD9, BOOL bSoftware)
    {
    DebugLine(_T("SetSoftwareVertexProcessing(%08X)"), bSoftware);
    return SetSoftwareVertexProcessingOrig(pD3DD9, bSoftware);
    }

BOOL STDMETHODCALLTYPE GetSoftwareVertexProcessingHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("GetSoftwareVertexProcessing()"));
    return GetSoftwareVertexProcessingOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE SetNPatchModeHook(IDirect3DDevice9* pD3DD9, float nSegments)
    {
    DebugLine(_T("SetNPatchMode(%0.2f)"), (double)nSegments);
    return SetNPatchModeOrig(pD3DD9, nSegments);
    }

float STDMETHODCALLTYPE GetNPatchModeHook(IDirect3DDevice9* pD3DD9)
    {
    DebugLine(_T("GetNPatchMode()"));
    return GetNPatchModeOrig(pD3DD9);
    }

HRESULT STDMETHODCALLTYPE DrawPrimitiveHook(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT StartVertex, UINT PrimitiveCount)
    {
    DebugLine(_T("DrawPrimitive(%08X, %08X, %08X)"), PrimitiveType, StartVertex, PrimitiveCount);
    DebugLine(_T(""));
    return DrawPrimitiveOrig(pD3DD9, PrimitiveType, StartVertex, PrimitiveCount);
    }

HRESULT STDMETHODCALLTYPE DrawIndexedPrimitiveHook(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE arg0, INT BaseVertexIndex, UINT MinVertexIndex, UINT NumVertices, UINT startIndex, UINT primCount)
    {
    DebugLine(_T("DrawIndexedPrimitive(%08X, %08X, %08X, %08X, %08X, %08X)"), arg0, BaseVertexIndex, MinVertexIndex, NumVertices, startIndex, primCount);
    DebugLine(_T(""));
    return DrawIndexedPrimitiveOrig(pD3DD9, arg0, BaseVertexIndex, MinVertexIndex, NumVertices, startIndex, primCount);
    }

HRESULT STDMETHODCALLTYPE DrawPrimitiveUPHook(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT PrimitiveCount, CONST void* pVertexStreamZeroData, UINT VertexStreamZeroStride)
    {
    DebugLine(_T("DrawPrimitiveUP(%08X, %08X, %08X, %08X)"), PrimitiveType, PrimitiveCount, pVertexStreamZeroData, VertexStreamZeroStride);
    DebugLine(_T(""));
    return DrawPrimitiveUPOrig(pD3DD9, PrimitiveType, PrimitiveCount, pVertexStreamZeroData, VertexStreamZeroStride);
    }

HRESULT STDMETHODCALLTYPE DrawIndexedPrimitiveUPHook(IDirect3DDevice9* pD3DD9, D3DPRIMITIVETYPE PrimitiveType, UINT MinVertexIndex, UINT NumVertices, UINT PrimitiveCount, CONST void* pIndexData, D3DFORMAT IndexDataFormat, CONST void* pVertexStreamZeroData, UINT VertexStreamZeroStride)
    {
    DebugLine(_T("DrawIndexedPrimitiveUP(%08X, %08X, %08X, %08X, %08X, %08X, %08X, %08X)"), PrimitiveType, MinVertexIndex, NumVertices, PrimitiveCount, pIndexData, IndexDataFormat, pVertexStreamZeroData, VertexStreamZeroStride);
    DebugLine(_T(""));
    return DrawIndexedPrimitiveUPOrig(pD3DD9, PrimitiveType, MinVertexIndex, NumVertices, PrimitiveCount, pIndexData, IndexDataFormat, pVertexStreamZeroData, VertexStreamZeroStride);
    }

HRESULT STDMETHODCALLTYPE ProcessVerticesHook(IDirect3DDevice9* pD3DD9, UINT SrcStartIndex, UINT DestIndex, UINT VertexCount, IDirect3DVertexBuffer9* pDestBuffer, IDirect3DVertexDeclaration9* pVertexDecl, DWORD Flags)
    {
    DebugLine(_T("ProcessVertices(%08X, %08X, %08X, %08X, %08X, %08X)"), SrcStartIndex, DestIndex, VertexCount, pDestBuffer, pVertexDecl, Flags);
    return ProcessVerticesOrig(pD3DD9, SrcStartIndex, DestIndex, VertexCount, pDestBuffer, pVertexDecl, Flags);
    }

HRESULT STDMETHODCALLTYPE CreateVertexDeclarationHook(IDirect3DDevice9* pD3DD9, CONST D3DVERTEXELEMENT9* pVertexElements, IDirect3DVertexDeclaration9** ppDecl)
    {
    DebugLine(_T("CreateVertexDeclaration(%08X, %08X)"), pVertexElements, ppDecl);
    return CreateVertexDeclarationOrig(pD3DD9, pVertexElements, ppDecl);
    }

HRESULT STDMETHODCALLTYPE SetVertexDeclarationHook(IDirect3DDevice9* pD3DD9, IDirect3DVertexDeclaration9* pDecl)
    {
    DebugLine(_T("SetVertexDeclaration(%08X)"), pDecl);
    return SetVertexDeclarationOrig(pD3DD9, pDecl);
    }

HRESULT STDMETHODCALLTYPE GetVertexDeclarationHook(IDirect3DDevice9* pD3DD9, IDirect3DVertexDeclaration9** ppDecl)
    {
    DebugLine(_T("GetVertexDeclaration(%08X)"), ppDecl);
    return GetVertexDeclarationOrig(pD3DD9, ppDecl);
    }

HRESULT STDMETHODCALLTYPE SetFVFHook(IDirect3DDevice9* pD3DD9, DWORD FVF)
    {
    DebugLine(_T("SetFVF(%08X)"), FVF);
    return SetFVFOrig(pD3DD9, FVF);
    }

HRESULT STDMETHODCALLTYPE GetFVFHook(IDirect3DDevice9* pD3DD9, DWORD* pFVF)
    {
    DebugLine(_T("GetFVF(%08X)"), pFVF);
    return GetFVFOrig(pD3DD9, pFVF);
    }

HRESULT STDMETHODCALLTYPE CreateVertexShaderHook(IDirect3DDevice9* pD3DD9, CONST DWORD* pFunction, IDirect3DVertexShader9** ppShader)
    {
    DebugLine(_T("CreateVertexShader(%08X, %08X)"), pFunction, ppShader);
    return CreateVertexShaderOrig(pD3DD9, pFunction, ppShader);
    }

HRESULT STDMETHODCALLTYPE SetVertexShaderHook(IDirect3DDevice9* pD3DD9, IDirect3DVertexShader9* pShader)
    {
    DebugLine(_T("SetVertexShader(%08X)"), pShader);
    return SetVertexShaderOrig(pD3DD9, pShader);
    }

HRESULT STDMETHODCALLTYPE GetVertexShaderHook(IDirect3DDevice9* pD3DD9, IDirect3DVertexShader9** ppShader)
    {
    DebugLine(_T("GetVertexShader(%08X)"), ppShader);
    return GetVertexShaderOrig(pD3DD9, ppShader);
    }

HRESULT STDMETHODCALLTYPE SetVertexShaderConstantFHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST float* pConstantData, UINT Vector4fCount)
    {
    DebugLine(_T("SetVertexShaderConstantF(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4fCount);
    return SetVertexShaderConstantFOrig(pD3DD9, StartRegister, pConstantData, Vector4fCount);
    }

HRESULT STDMETHODCALLTYPE GetVertexShaderConstantFHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, float* pConstantData, UINT Vector4fCount)
    {
    DebugLine(_T("GetVertexShaderConstantF(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4fCount);
    return GetVertexShaderConstantFOrig(pD3DD9, StartRegister, pConstantData, Vector4fCount);
    }

HRESULT STDMETHODCALLTYPE SetVertexShaderConstantIHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST int* pConstantData, UINT Vector4iCount)
    {
    DebugLine(_T("SetVertexShaderConstantI(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4iCount);
    return SetVertexShaderConstantIOrig(pD3DD9, StartRegister, pConstantData, Vector4iCount);
    }

HRESULT STDMETHODCALLTYPE GetVertexShaderConstantIHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, int* pConstantData, UINT Vector4iCount)
    {
    DebugLine(_T("GetVertexShaderConstantI(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4iCount);
    return GetVertexShaderConstantIOrig(pD3DD9, StartRegister, pConstantData, Vector4iCount);
    }

HRESULT STDMETHODCALLTYPE SetVertexShaderConstantBHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST BOOL* pConstantData, UINT BoolCount)
    {
    DebugLine(_T("SetVertexShaderConstantB(%08X, %08X, %08X)"), StartRegister, pConstantData, BoolCount);
    return SetVertexShaderConstantBOrig(pD3DD9, StartRegister, pConstantData, BoolCount);
    }

HRESULT STDMETHODCALLTYPE GetVertexShaderConstantBHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, BOOL* pConstantData, UINT BoolCount)
    {
    DebugLine(_T("GetVertexShaderConstantB(%08X, %08X, %08X)"), StartRegister, pConstantData, BoolCount);
    return GetVertexShaderConstantBOrig(pD3DD9, StartRegister, pConstantData, BoolCount);
    }

HRESULT STDMETHODCALLTYPE SetStreamSourceHook(IDirect3DDevice9* pD3DD9, UINT StreamNumber, IDirect3DVertexBuffer9* pStreamData, UINT OffsetInBytes, UINT Stride)
    {
    DebugLine(_T("SetStreamSource(%08X, %08X, %08X, %08X)"), StreamNumber, pStreamData, OffsetInBytes, Stride);
    return SetStreamSourceOrig(pD3DD9, StreamNumber, pStreamData, OffsetInBytes, Stride);
    }

HRESULT STDMETHODCALLTYPE GetStreamSourceHook(IDirect3DDevice9* pD3DD9, UINT StreamNumber, IDirect3DVertexBuffer9** ppStreamData, UINT* pOffsetInBytes, UINT* pStride)
    {
    DebugLine(_T("GetStreamSource(%08X, %08X, %08X, %08X)"), StreamNumber, ppStreamData, pOffsetInBytes, pStride);
    return GetStreamSourceOrig(pD3DD9, StreamNumber, ppStreamData, pOffsetInBytes, pStride);
    }

HRESULT STDMETHODCALLTYPE SetStreamSourceFreqHook(IDirect3DDevice9* pD3DD9, UINT StreamNumber, UINT Setting)
    {
    DebugLine(_T("SetStreamSourceFreq(%08X, %08X)"), StreamNumber, Setting);
    return SetStreamSourceFreqOrig(pD3DD9, StreamNumber, Setting);
    }

HRESULT STDMETHODCALLTYPE GetStreamSourceFreqHook(IDirect3DDevice9* pD3DD9, UINT StreamNumber, UINT* pSetting)
    {
    DebugLine(_T("GetStreamSourceFreq(%08X, %08X)"), StreamNumber, pSetting);
    return GetStreamSourceFreqOrig(pD3DD9, StreamNumber, pSetting);
    }

HRESULT STDMETHODCALLTYPE SetIndicesHook(IDirect3DDevice9* pD3DD9, IDirect3DIndexBuffer9* pIndexData)
    {
    DebugLine(_T("SetIndices(%08X)"), pIndexData);
    return SetIndicesOrig(pD3DD9, pIndexData);
    }

HRESULT STDMETHODCALLTYPE GetIndicesHook(IDirect3DDevice9* pD3DD9, IDirect3DIndexBuffer9** ppIndexData)
    {
    DebugLine(_T("GetIndices(%08X)"), ppIndexData);
    return GetIndicesOrig(pD3DD9, ppIndexData);
    }

HRESULT STDMETHODCALLTYPE CreatePixelShaderHook(IDirect3DDevice9* pD3DD9, CONST DWORD* pFunction, IDirect3DPixelShader9** ppShader)
    {
    DebugLine(_T("CreatePixelShader(%08X, %08X)"), pFunction, ppShader);
    return CreatePixelShaderOrig(pD3DD9, pFunction, ppShader);
    }

HRESULT STDMETHODCALLTYPE SetPixelShaderHook(IDirect3DDevice9* pD3DD9, IDirect3DPixelShader9* pShader)
    {
    DebugLine(_T("SetPixelShader(%08X)"), pShader);
    return SetPixelShaderOrig(pD3DD9, pShader);
    }

HRESULT STDMETHODCALLTYPE GetPixelShaderHook(IDirect3DDevice9* pD3DD9, IDirect3DPixelShader9** ppShader)
    {
    DebugLine(_T("GetPixelShader(%08X)"), ppShader);
    return GetPixelShaderOrig(pD3DD9, ppShader);
    }

HRESULT STDMETHODCALLTYPE SetPixelShaderConstantFHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST float* pConstantData, UINT Vector4fCount)
    {
    DebugLine(_T("SetPixelShaderConstantF(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4fCount);
    return SetPixelShaderConstantFOrig(pD3DD9, StartRegister, pConstantData, Vector4fCount);
    }

HRESULT STDMETHODCALLTYPE GetPixelShaderConstantFHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, float* pConstantData, UINT Vector4fCount)
    {
    DebugLine(_T("GetPixelShaderConstantF(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4fCount);
    return GetPixelShaderConstantFOrig(pD3DD9, StartRegister, pConstantData, Vector4fCount);
    }

HRESULT STDMETHODCALLTYPE SetPixelShaderConstantIHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST int* pConstantData, UINT Vector4iCount)
    {
    DebugLine(_T("SetPixelShaderConstantI(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4iCount);
    return SetPixelShaderConstantIOrig(pD3DD9, StartRegister, pConstantData, Vector4iCount);
    }

HRESULT STDMETHODCALLTYPE GetPixelShaderConstantIHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, int* pConstantData, UINT Vector4iCount)
    {
    DebugLine(_T("GetPixelShaderConstantI(%08X, %08X, %08X)"), StartRegister, pConstantData, Vector4iCount);
    return GetPixelShaderConstantIOrig(pD3DD9, StartRegister, pConstantData, Vector4iCount);
    }

HRESULT STDMETHODCALLTYPE SetPixelShaderConstantBHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, CONST BOOL* pConstantData, UINT BoolCount)
    {
    DebugLine(_T("SetPixelShaderConstantB(%08X, %08X, %08X)"), StartRegister, pConstantData, BoolCount);
    return SetPixelShaderConstantBOrig(pD3DD9, StartRegister, pConstantData, BoolCount);
    }

HRESULT STDMETHODCALLTYPE GetPixelShaderConstantBHook(IDirect3DDevice9* pD3DD9, UINT StartRegister, BOOL* pConstantData, UINT BoolCount)
    {
    DebugLine(_T("GetPixelShaderConstantB(%08X, %08X, %08X)"), StartRegister, pConstantData, BoolCount);
    return GetPixelShaderConstantBOrig(pD3DD9, StartRegister, pConstantData, BoolCount);
    }

HRESULT STDMETHODCALLTYPE DrawRectPatchHook(IDirect3DDevice9* pD3DD9, UINT Handle, CONST float* pNumSegs, CONST D3DRECTPATCH_INFO* pRectPatchInfo)
    {
    DebugLine(_T("DrawRectPatch(%08X, %08X, %08X)"), Handle, pNumSegs, pRectPatchInfo);
    return DrawRectPatchOrig(pD3DD9, Handle, pNumSegs, pRectPatchInfo);
    }

HRESULT STDMETHODCALLTYPE DrawTriPatchHook(IDirect3DDevice9* pD3DD9, UINT Handle, CONST float* pNumSegs, CONST D3DTRIPATCH_INFO* pTriPatchInfo)
    {
    DebugLine(_T("DrawTriPatch(%08X, %08X, %08X)"), Handle, pNumSegs, pTriPatchInfo);
    return DrawTriPatchOrig(pD3DD9, Handle, pNumSegs, pTriPatchInfo);
    }

HRESULT STDMETHODCALLTYPE DeletePatchHook(IDirect3DDevice9* pD3DD9, UINT Handle)
    {
    DebugLine(_T("DeletePatch(%08X)"), Handle);
    return DeletePatchOrig(pD3DD9, Handle);
    }

HRESULT STDMETHODCALLTYPE CreateQueryHook(IDirect3DDevice9* pD3DD9, D3DQUERYTYPE Type, IDirect3DQuery9** ppQuery)
    {
    DebugLine(_T("CreateQuery(%08X, %08X)"), Type, ppQuery);
    return CreateQueryOrig(pD3DD9, Type, ppQuery);
    }


void HookAllMethods(IDirect3DDevice9* pD3DD9)
    {
    HookVTable(pD3DD9, 3, &TestCooperativeLevelOrig, TestCooperativeLevelHook);
    HookVTable(pD3DD9, 4, &GetAvailableTextureMemOrig, GetAvailableTextureMemHook);
    HookVTable(pD3DD9, 5, &EvictManagedResourcesOrig, EvictManagedResourcesHook);
    HookVTable(pD3DD9, 6, &GetDirect3DOrig, GetDirect3DHook);
    HookVTable(pD3DD9, 7, &GetDeviceCapsOrig, GetDeviceCapsHook);
    HookVTable(pD3DD9, 8, &GetDisplayModeOrig, GetDisplayModeHook);
    HookVTable(pD3DD9, 9, &GetCreationParametersOrig, GetCreationParametersHook);
    HookVTable(pD3DD9, 10, &SetCursorPropertiesOrig, SetCursorPropertiesHook);
    HookVTable(pD3DD9, 11, &SetCursorPositionOrig, SetCursorPositionHook);
    HookVTable(pD3DD9, 12, &ShowCursorOrig, ShowCursorHook);
    HookVTable(pD3DD9, 13, &CreateAdditionalSwapChainOrig, CreateAdditionalSwapChainHook);
    HookVTable(pD3DD9, 14, &GetSwapChainOrig, GetSwapChainHook);
    HookVTable(pD3DD9, 15, &GetNumberOfSwapChainsOrig, GetNumberOfSwapChainsHook);
    HookVTable(pD3DD9, 16, &ResetOrig, ResetHook);
    HookVTable(pD3DD9, 17, &PresentOrig, PresentHook);
    HookVTable(pD3DD9, 18, &GetBackBufferOrig, GetBackBufferHook);
    HookVTable(pD3DD9, 19, &GetRasterStatusOrig, GetRasterStatusHook);
    HookVTable(pD3DD9, 20, &SetDialogBoxModeOrig, SetDialogBoxModeHook);
    HookVTable(pD3DD9, 21, &SetGammaRampOrig, SetGammaRampHook);
    HookVTable(pD3DD9, 22, &GetGammaRampOrig, GetGammaRampHook);
    HookVTable(pD3DD9, 23, &CreateTextureOrig, CreateTextureHook);
    HookVTable(pD3DD9, 24, &CreateVolumeTextureOrig, CreateVolumeTextureHook);
    HookVTable(pD3DD9, 25, &CreateCubeTextureOrig, CreateCubeTextureHook);
    HookVTable(pD3DD9, 26, &CreateVertexBufferOrig, CreateVertexBufferHook);
    HookVTable(pD3DD9, 27, &CreateIndexBufferOrig, CreateIndexBufferHook);
    HookVTable(pD3DD9, 28, &CreateRenderTargetOrig, CreateRenderTargetHook);
    HookVTable(pD3DD9, 29, &CreateDepthStencilSurfaceOrig, CreateDepthStencilSurfaceHook);
    HookVTable(pD3DD9, 30, &UpdateSurfaceOrig, UpdateSurfaceHook);
    HookVTable(pD3DD9, 31, &UpdateTextureOrig, UpdateTextureHook);
    HookVTable(pD3DD9, 32, &GetRenderTargetDataOrig, GetRenderTargetDataHook);
    HookVTable(pD3DD9, 33, &GetFrontBufferDataOrig, GetFrontBufferDataHook);
    HookVTable(pD3DD9, 34, &StretchRectOrig, StretchRectHook);
    HookVTable(pD3DD9, 35, &ColorFillOrig, ColorFillHook);
    HookVTable(pD3DD9, 36, &CreateOffscreenPlainSurfaceOrig, CreateOffscreenPlainSurfaceHook);
    HookVTable(pD3DD9, 37, &SetRenderTargetOrig, SetRenderTargetHook);
    HookVTable(pD3DD9, 38, &GetRenderTargetOrig, GetRenderTargetHook);
    HookVTable(pD3DD9, 39, &SetDepthStencilSurfaceOrig, SetDepthStencilSurfaceHook);
    HookVTable(pD3DD9, 40, &GetDepthStencilSurfaceOrig, GetDepthStencilSurfaceHook);
    HookVTable(pD3DD9, 41, &BeginSceneOrig, BeginSceneHook);
    HookVTable(pD3DD9, 42, &EndSceneOrig, EndSceneHook);
    HookVTable(pD3DD9, 43, &ClearOrig, ClearHook);
    HookVTable(pD3DD9, 44, &SetTransformOrig, SetTransformHook);
    HookVTable(pD3DD9, 45, &GetTransformOrig, GetTransformHook);
    HookVTable(pD3DD9, 46, &MultiplyTransformOrig, MultiplyTransformHook);
    HookVTable(pD3DD9, 47, &SetViewportOrig, SetViewportHook);
    HookVTable(pD3DD9, 48, &GetViewportOrig, GetViewportHook);
    HookVTable(pD3DD9, 49, &SetMaterialOrig, SetMaterialHook);
    HookVTable(pD3DD9, 50, &GetMaterialOrig, GetMaterialHook);
    HookVTable(pD3DD9, 51, &SetLightOrig, SetLightHook);
    HookVTable(pD3DD9, 52, &GetLightOrig, GetLightHook);
    HookVTable(pD3DD9, 53, &LightEnableOrig, LightEnableHook);
    HookVTable(pD3DD9, 54, &GetLightEnableOrig, GetLightEnableHook);
    HookVTable(pD3DD9, 55, &SetClipPlaneOrig, SetClipPlaneHook);
    HookVTable(pD3DD9, 56, &GetClipPlaneOrig, GetClipPlaneHook);
    HookVTable(pD3DD9, 57, &SetRenderStateOrig, SetRenderStateHook);
    HookVTable(pD3DD9, 58, &GetRenderStateOrig, GetRenderStateHook);
    HookVTable(pD3DD9, 59, &CreateStateBlockOrig, CreateStateBlockHook);
    HookVTable(pD3DD9, 60, &BeginStateBlockOrig, BeginStateBlockHook);
    HookVTable(pD3DD9, 61, &EndStateBlockOrig, EndStateBlockHook);
    HookVTable(pD3DD9, 62, &SetClipStatusOrig, SetClipStatusHook);
    HookVTable(pD3DD9, 63, &GetClipStatusOrig, GetClipStatusHook);
    HookVTable(pD3DD9, 64, &GetTextureOrig, GetTextureHook);
    HookVTable(pD3DD9, 65, &SetTextureOrig, SetTextureHook);
    HookVTable(pD3DD9, 66, &GetTextureStageStateOrig, GetTextureStageStateHook);
    HookVTable(pD3DD9, 67, &SetTextureStageStateOrig, SetTextureStageStateHook);
    HookVTable(pD3DD9, 68, &GetSamplerStateOrig, GetSamplerStateHook);
    HookVTable(pD3DD9, 69, &SetSamplerStateOrig, SetSamplerStateHook);
    HookVTable(pD3DD9, 70, &ValidateDeviceOrig, ValidateDeviceHook);
    HookVTable(pD3DD9, 71, &SetPaletteEntriesOrig, SetPaletteEntriesHook);
    HookVTable(pD3DD9, 72, &GetPaletteEntriesOrig, GetPaletteEntriesHook);
    HookVTable(pD3DD9, 73, &SetCurrentTexturePaletteOrig, SetCurrentTexturePaletteHook);
    HookVTable(pD3DD9, 74, &GetCurrentTexturePaletteOrig, GetCurrentTexturePaletteHook);
    HookVTable(pD3DD9, 75, &SetScissorRectOrig, SetScissorRectHook);
    HookVTable(pD3DD9, 76, &GetScissorRectOrig, GetScissorRectHook);
    HookVTable(pD3DD9, 77, &SetSoftwareVertexProcessingOrig, SetSoftwareVertexProcessingHook);
    HookVTable(pD3DD9, 78, &GetSoftwareVertexProcessingOrig, GetSoftwareVertexProcessingHook);
    HookVTable(pD3DD9, 79, &SetNPatchModeOrig, SetNPatchModeHook);
    HookVTable(pD3DD9, 80, &GetNPatchModeOrig, GetNPatchModeHook);
    HookVTable(pD3DD9, 81, &DrawPrimitiveOrig, DrawPrimitiveHook);
    HookVTable(pD3DD9, 82, &DrawIndexedPrimitiveOrig, DrawIndexedPrimitiveHook);
    HookVTable(pD3DD9, 83, &DrawPrimitiveUPOrig, DrawPrimitiveUPHook);
    HookVTable(pD3DD9, 84, &DrawIndexedPrimitiveUPOrig, DrawIndexedPrimitiveUPHook);
    HookVTable(pD3DD9, 85, &ProcessVerticesOrig, ProcessVerticesHook);
    HookVTable(pD3DD9, 86, &CreateVertexDeclarationOrig, CreateVertexDeclarationHook);
    HookVTable(pD3DD9, 87, &SetVertexDeclarationOrig, SetVertexDeclarationHook);
    HookVTable(pD3DD9, 88, &GetVertexDeclarationOrig, GetVertexDeclarationHook);
    HookVTable(pD3DD9, 89, &SetFVFOrig, SetFVFHook);
    HookVTable(pD3DD9, 90, &GetFVFOrig, GetFVFHook);
    HookVTable(pD3DD9, 91, &CreateVertexShaderOrig, CreateVertexShaderHook);
    HookVTable(pD3DD9, 92, &SetVertexShaderOrig, SetVertexShaderHook);
    HookVTable(pD3DD9, 93, &GetVertexShaderOrig, GetVertexShaderHook);
    HookVTable(pD3DD9, 94, &SetVertexShaderConstantFOrig, SetVertexShaderConstantFHook);
    HookVTable(pD3DD9, 95, &GetVertexShaderConstantFOrig, GetVertexShaderConstantFHook);
    HookVTable(pD3DD9, 96, &SetVertexShaderConstantIOrig, SetVertexShaderConstantIHook);
    HookVTable(pD3DD9, 97, &GetVertexShaderConstantIOrig, GetVertexShaderConstantIHook);
    HookVTable(pD3DD9, 98, &SetVertexShaderConstantBOrig, SetVertexShaderConstantBHook);
    HookVTable(pD3DD9, 99, &GetVertexShaderConstantBOrig, GetVertexShaderConstantBHook);
    HookVTable(pD3DD9, 100, &SetStreamSourceOrig, SetStreamSourceHook);
    HookVTable(pD3DD9, 101, &GetStreamSourceOrig, GetStreamSourceHook);
    HookVTable(pD3DD9, 102, &SetStreamSourceFreqOrig, SetStreamSourceFreqHook);
    HookVTable(pD3DD9, 103, &GetStreamSourceFreqOrig, GetStreamSourceFreqHook);
    HookVTable(pD3DD9, 104, &SetIndicesOrig, SetIndicesHook);
    HookVTable(pD3DD9, 105, &GetIndicesOrig, GetIndicesHook);
    HookVTable(pD3DD9, 106, &CreatePixelShaderOrig, CreatePixelShaderHook);
    HookVTable(pD3DD9, 107, &SetPixelShaderOrig, SetPixelShaderHook);
    HookVTable(pD3DD9, 108, &GetPixelShaderOrig, GetPixelShaderHook);
    HookVTable(pD3DD9, 109, &SetPixelShaderConstantFOrig, SetPixelShaderConstantFHook);
    HookVTable(pD3DD9, 110, &GetPixelShaderConstantFOrig, GetPixelShaderConstantFHook);
    HookVTable(pD3DD9, 111, &SetPixelShaderConstantIOrig, SetPixelShaderConstantIHook);
    HookVTable(pD3DD9, 112, &GetPixelShaderConstantIOrig, GetPixelShaderConstantIHook);
    HookVTable(pD3DD9, 113, &SetPixelShaderConstantBOrig, SetPixelShaderConstantBHook);
    HookVTable(pD3DD9, 114, &GetPixelShaderConstantBOrig, GetPixelShaderConstantBHook);
    HookVTable(pD3DD9, 115, &DrawRectPatchOrig, DrawRectPatchHook);
    HookVTable(pD3DD9, 116, &DrawTriPatchOrig, DrawTriPatchHook);
    HookVTable(pD3DD9, 117, &DeletePatchOrig, DeletePatchHook);
    HookVTable(pD3DD9, 118, &CreateQueryOrig, CreateQueryHook);
    }

void UnhookAllMethods(IDirect3DDevice9* pD3DD9)
    {
    UnhookVTable(pD3DD9, 3, TestCooperativeLevelOrig, TestCooperativeLevelHook);
    UnhookVTable(pD3DD9, 4, GetAvailableTextureMemOrig, GetAvailableTextureMemHook);
    UnhookVTable(pD3DD9, 5, EvictManagedResourcesOrig, EvictManagedResourcesHook);
    UnhookVTable(pD3DD9, 6, GetDirect3DOrig, GetDirect3DHook);
    UnhookVTable(pD3DD9, 7, GetDeviceCapsOrig, GetDeviceCapsHook);
    UnhookVTable(pD3DD9, 8, GetDisplayModeOrig, GetDisplayModeHook);
    UnhookVTable(pD3DD9, 9, GetCreationParametersOrig, GetCreationParametersHook);
    UnhookVTable(pD3DD9, 10, SetCursorPropertiesOrig, SetCursorPropertiesHook);
    UnhookVTable(pD3DD9, 11, SetCursorPositionOrig, SetCursorPositionHook);
    UnhookVTable(pD3DD9, 12, ShowCursorOrig, ShowCursorHook);
    UnhookVTable(pD3DD9, 13, CreateAdditionalSwapChainOrig, CreateAdditionalSwapChainHook);
    UnhookVTable(pD3DD9, 14, GetSwapChainOrig, GetSwapChainHook);
    UnhookVTable(pD3DD9, 15, GetNumberOfSwapChainsOrig, GetNumberOfSwapChainsHook);
    UnhookVTable(pD3DD9, 16, ResetOrig, ResetHook);
    UnhookVTable(pD3DD9, 17, PresentOrig, PresentHook);
    UnhookVTable(pD3DD9, 18, GetBackBufferOrig, GetBackBufferHook);
    UnhookVTable(pD3DD9, 19, GetRasterStatusOrig, GetRasterStatusHook);
    UnhookVTable(pD3DD9, 20, SetDialogBoxModeOrig, SetDialogBoxModeHook);
    UnhookVTable(pD3DD9, 21, SetGammaRampOrig, SetGammaRampHook);
    UnhookVTable(pD3DD9, 22, GetGammaRampOrig, GetGammaRampHook);
    UnhookVTable(pD3DD9, 23, CreateTextureOrig, CreateTextureHook);
    UnhookVTable(pD3DD9, 24, CreateVolumeTextureOrig, CreateVolumeTextureHook);
    UnhookVTable(pD3DD9, 25, CreateCubeTextureOrig, CreateCubeTextureHook);
    UnhookVTable(pD3DD9, 26, CreateVertexBufferOrig, CreateVertexBufferHook);
    UnhookVTable(pD3DD9, 27, CreateIndexBufferOrig, CreateIndexBufferHook);
    UnhookVTable(pD3DD9, 28, CreateRenderTargetOrig, CreateRenderTargetHook);
    UnhookVTable(pD3DD9, 29, CreateDepthStencilSurfaceOrig, CreateDepthStencilSurfaceHook);
    UnhookVTable(pD3DD9, 30, UpdateSurfaceOrig, UpdateSurfaceHook);
    UnhookVTable(pD3DD9, 31, UpdateTextureOrig, UpdateTextureHook);
    UnhookVTable(pD3DD9, 32, GetRenderTargetDataOrig, GetRenderTargetDataHook);
    UnhookVTable(pD3DD9, 33, GetFrontBufferDataOrig, GetFrontBufferDataHook);
    UnhookVTable(pD3DD9, 34, StretchRectOrig, StretchRectHook);
    UnhookVTable(pD3DD9, 35, ColorFillOrig, ColorFillHook);
    UnhookVTable(pD3DD9, 36, CreateOffscreenPlainSurfaceOrig, CreateOffscreenPlainSurfaceHook);
    UnhookVTable(pD3DD9, 37, SetRenderTargetOrig, SetRenderTargetHook);
    UnhookVTable(pD3DD9, 38, GetRenderTargetOrig, GetRenderTargetHook);
    UnhookVTable(pD3DD9, 39, SetDepthStencilSurfaceOrig, SetDepthStencilSurfaceHook);
    UnhookVTable(pD3DD9, 40, GetDepthStencilSurfaceOrig, GetDepthStencilSurfaceHook);
    UnhookVTable(pD3DD9, 41, BeginSceneOrig, BeginSceneHook);
    UnhookVTable(pD3DD9, 42, EndSceneOrig, EndSceneHook);
    UnhookVTable(pD3DD9, 43, ClearOrig, ClearHook);
    UnhookVTable(pD3DD9, 44, SetTransformOrig, SetTransformHook);
    UnhookVTable(pD3DD9, 45, GetTransformOrig, GetTransformHook);
    UnhookVTable(pD3DD9, 46, MultiplyTransformOrig, MultiplyTransformHook);
    UnhookVTable(pD3DD9, 47, SetViewportOrig, SetViewportHook);
    UnhookVTable(pD3DD9, 48, GetViewportOrig, GetViewportHook);
    UnhookVTable(pD3DD9, 49, SetMaterialOrig, SetMaterialHook);
    UnhookVTable(pD3DD9, 50, GetMaterialOrig, GetMaterialHook);
    UnhookVTable(pD3DD9, 51, SetLightOrig, SetLightHook);
    UnhookVTable(pD3DD9, 52, GetLightOrig, GetLightHook);
    UnhookVTable(pD3DD9, 53, LightEnableOrig, LightEnableHook);
    UnhookVTable(pD3DD9, 54, GetLightEnableOrig, GetLightEnableHook);
    UnhookVTable(pD3DD9, 55, SetClipPlaneOrig, SetClipPlaneHook);
    UnhookVTable(pD3DD9, 56, GetClipPlaneOrig, GetClipPlaneHook);
    UnhookVTable(pD3DD9, 57, SetRenderStateOrig, SetRenderStateHook);
    UnhookVTable(pD3DD9, 58, GetRenderStateOrig, GetRenderStateHook);
    UnhookVTable(pD3DD9, 59, CreateStateBlockOrig, CreateStateBlockHook);
    UnhookVTable(pD3DD9, 60, BeginStateBlockOrig, BeginStateBlockHook);
    UnhookVTable(pD3DD9, 61, EndStateBlockOrig, EndStateBlockHook);
    UnhookVTable(pD3DD9, 62, SetClipStatusOrig, SetClipStatusHook);
    UnhookVTable(pD3DD9, 63, GetClipStatusOrig, GetClipStatusHook);
    UnhookVTable(pD3DD9, 64, GetTextureOrig, GetTextureHook);
    UnhookVTable(pD3DD9, 65, SetTextureOrig, SetTextureHook);
    UnhookVTable(pD3DD9, 66, GetTextureStageStateOrig, GetTextureStageStateHook);
    UnhookVTable(pD3DD9, 67, SetTextureStageStateOrig, SetTextureStageStateHook);
    UnhookVTable(pD3DD9, 68, GetSamplerStateOrig, GetSamplerStateHook);
    UnhookVTable(pD3DD9, 69, SetSamplerStateOrig, SetSamplerStateHook);
    UnhookVTable(pD3DD9, 70, ValidateDeviceOrig, ValidateDeviceHook);
    UnhookVTable(pD3DD9, 71, SetPaletteEntriesOrig, SetPaletteEntriesHook);
    UnhookVTable(pD3DD9, 72, GetPaletteEntriesOrig, GetPaletteEntriesHook);
    UnhookVTable(pD3DD9, 73, SetCurrentTexturePaletteOrig, SetCurrentTexturePaletteHook);
    UnhookVTable(pD3DD9, 74, GetCurrentTexturePaletteOrig, GetCurrentTexturePaletteHook);
    UnhookVTable(pD3DD9, 75, SetScissorRectOrig, SetScissorRectHook);
    UnhookVTable(pD3DD9, 76, GetScissorRectOrig, GetScissorRectHook);
    UnhookVTable(pD3DD9, 77, SetSoftwareVertexProcessingOrig, SetSoftwareVertexProcessingHook);
    UnhookVTable(pD3DD9, 78, GetSoftwareVertexProcessingOrig, GetSoftwareVertexProcessingHook);
    UnhookVTable(pD3DD9, 79, SetNPatchModeOrig, SetNPatchModeHook);
    UnhookVTable(pD3DD9, 80, GetNPatchModeOrig, GetNPatchModeHook);
    UnhookVTable(pD3DD9, 81, DrawPrimitiveOrig, DrawPrimitiveHook);
    UnhookVTable(pD3DD9, 82, DrawIndexedPrimitiveOrig, DrawIndexedPrimitiveHook);
    UnhookVTable(pD3DD9, 83, DrawPrimitiveUPOrig, DrawPrimitiveUPHook);
    UnhookVTable(pD3DD9, 84, DrawIndexedPrimitiveUPOrig, DrawIndexedPrimitiveUPHook);
    UnhookVTable(pD3DD9, 85, ProcessVerticesOrig, ProcessVerticesHook);
    UnhookVTable(pD3DD9, 86, CreateVertexDeclarationOrig, CreateVertexDeclarationHook);
    UnhookVTable(pD3DD9, 87, SetVertexDeclarationOrig, SetVertexDeclarationHook);
    UnhookVTable(pD3DD9, 88, GetVertexDeclarationOrig, GetVertexDeclarationHook);
    UnhookVTable(pD3DD9, 89, SetFVFOrig, SetFVFHook);
    UnhookVTable(pD3DD9, 90, GetFVFOrig, GetFVFHook);
    UnhookVTable(pD3DD9, 91, CreateVertexShaderOrig, CreateVertexShaderHook);
    UnhookVTable(pD3DD9, 92, SetVertexShaderOrig, SetVertexShaderHook);
    UnhookVTable(pD3DD9, 93, GetVertexShaderOrig, GetVertexShaderHook);
    UnhookVTable(pD3DD9, 94, SetVertexShaderConstantFOrig, SetVertexShaderConstantFHook);
    UnhookVTable(pD3DD9, 95, GetVertexShaderConstantFOrig, GetVertexShaderConstantFHook);
    UnhookVTable(pD3DD9, 96, SetVertexShaderConstantIOrig, SetVertexShaderConstantIHook);
    UnhookVTable(pD3DD9, 97, GetVertexShaderConstantIOrig, GetVertexShaderConstantIHook);
    UnhookVTable(pD3DD9, 98, SetVertexShaderConstantBOrig, SetVertexShaderConstantBHook);
    UnhookVTable(pD3DD9, 99, GetVertexShaderConstantBOrig, GetVertexShaderConstantBHook);
    UnhookVTable(pD3DD9, 100, SetStreamSourceOrig, SetStreamSourceHook);
    UnhookVTable(pD3DD9, 101, GetStreamSourceOrig, GetStreamSourceHook);
    UnhookVTable(pD3DD9, 102, SetStreamSourceFreqOrig, SetStreamSourceFreqHook);
    UnhookVTable(pD3DD9, 103, GetStreamSourceFreqOrig, GetStreamSourceFreqHook);
    UnhookVTable(pD3DD9, 104, SetIndicesOrig, SetIndicesHook);
    UnhookVTable(pD3DD9, 105, GetIndicesOrig, GetIndicesHook);
    UnhookVTable(pD3DD9, 106, CreatePixelShaderOrig, CreatePixelShaderHook);
    UnhookVTable(pD3DD9, 107, SetPixelShaderOrig, SetPixelShaderHook);
    UnhookVTable(pD3DD9, 108, GetPixelShaderOrig, GetPixelShaderHook);
    UnhookVTable(pD3DD9, 109, SetPixelShaderConstantFOrig, SetPixelShaderConstantFHook);
    UnhookVTable(pD3DD9, 110, GetPixelShaderConstantFOrig, GetPixelShaderConstantFHook);
    UnhookVTable(pD3DD9, 111, SetPixelShaderConstantIOrig, SetPixelShaderConstantIHook);
    UnhookVTable(pD3DD9, 112, GetPixelShaderConstantIOrig, GetPixelShaderConstantIHook);
    UnhookVTable(pD3DD9, 113, SetPixelShaderConstantBOrig, SetPixelShaderConstantBHook);
    UnhookVTable(pD3DD9, 114, GetPixelShaderConstantBOrig, GetPixelShaderConstantBHook);
    UnhookVTable(pD3DD9, 115, DrawRectPatchOrig, DrawRectPatchHook);
    UnhookVTable(pD3DD9, 116, DrawTriPatchOrig, DrawTriPatchHook);
    UnhookVTable(pD3DD9, 117, DeletePatchOrig, DeletePatchHook);
    UnhookVTable(pD3DD9, 118, CreateQueryOrig, CreateQueryHook);
    }


