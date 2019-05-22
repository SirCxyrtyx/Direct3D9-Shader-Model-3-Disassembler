# Direct3D9-Shader-Model-3-Disassembler

Not particularly useful yet, as it can't decode instructions.

Currently, if you compile this shader:
```
struct VertexShaderInput
{
    float3 pos : POSITION;
    float2 tex : TEXCOORD;
};

struct PixelShaderInput
{
    float4 pos : SV_POSITION;
    float2 tex : TEXCOORD;
};

float4x4 WorldViewProjection;

PixelShaderInput VS(VertexShaderInput input)
{
    PixelShaderInput output;

    output.pos = mul(float4(input.pos, 1), WorldViewProjection);
    output.tex = input.tex;

    return output;
}
```

Running this program on the result produces:
```
vs_3_0
Microsoft (R) HLSL Shader Compiler 9.29.952.3111


WorldViewProjection
RegisterSet: RS_FLOAT4, RegisterIndex: 0, RegisterCount: 4


def (instruction size: 5)
dcl (instruction size: 2)
dcl (instruction size: 2)
dcl (instruction size: 2)
dcl (instruction size: 2)
mad (instruction size: 4)
dp4 (instruction size: 3)
dp4 (instruction size: 3)
dp4 (instruction size: 3)
dp4 (instruction size: 3)
mov (instruction size: 2)
END

```

So, it needs some work.
