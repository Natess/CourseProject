using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomPipelineRender : RenderPipeline
{
    bool useDynamicBatching, useGPUInstancing;

    private CameraRenderer _cameraRenderer = new CameraRenderer();
    public CustomPipelineRender(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
		this.useDynamicBatching = useDynamicBatching;
		this.useGPUInstancing = useGPUInstancing;
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
		GraphicsSettings.lightsUseLinearIntensity = true;
    }
    protected override void Render(ScriptableRenderContext context,
        Camera[] cameras)
    {
        CamerasRender(context, cameras);
    }

    private void CamerasRender(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            _cameraRenderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
        }
    }
}

