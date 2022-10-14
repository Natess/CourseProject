using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CustomPipelineRenderAsset")]
public class CustomPipelineRenderAsset : RenderPipelineAsset
{
    [SerializeField]
	bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;

    protected override RenderPipeline CreatePipeline()
    {
		return new CustomPipelineRender(
			useDynamicBatching, useGPUInstancing, useSRPBatcher
		);
    }

}
