// Trace.h


LPCTSTR PszTraceLight(const D3DLIGHT9* plight);
LPCTSTR PszTraceMaterial(const D3DMATERIAL9* pmaterial);
LPCTSTR PszTraceMatrix(const D3DMATRIX* pmat);
LPCTSTR PszTraceViewport(const D3DVIEWPORT9* pviewport);
LPCTSTR PszTraceRenderState(D3DRENDERSTATETYPE rs);
LPCTSTR PszTraceSamplerState(D3DSAMPLERSTATETYPE ss);
LPCTSTR PszTraceTextureStageState(D3DTEXTURESTAGESTATETYPE tss);

void DumpMatrix(const D3DMATRIX* pmat);
void HexDump(const DWORD* prgdw, UINT cfloat, UINT cdw);
void DebugLine(LPCTSTR psz, ...);

