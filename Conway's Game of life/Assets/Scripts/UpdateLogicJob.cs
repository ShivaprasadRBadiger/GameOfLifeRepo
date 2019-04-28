using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

namespace GOL
{
	[BurstCompile(CompileSynchronously = true)]
	public struct UpdateLogicJob : IJob
	{
		NativeArray<Cell> cells;

		public void Execute()
		{
		}
	}
}
