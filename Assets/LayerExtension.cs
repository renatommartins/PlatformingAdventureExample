using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LayerExtension
{
	public static bool ContainsLayer(this LayerMask layerMask, int layer)
	{
		return ((1 << layer) & layerMask.value) != 0;
	}
}
