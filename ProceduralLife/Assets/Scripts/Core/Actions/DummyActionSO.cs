using MHLib.Actions;
using System;
using UnityEngine;

namespace ProceduralLife.Actions
{
    [CreateAssetMenu(fileName = "Dummy", menuName = Constants.Editor.PATH_ACTIONS + "Dummy")]
    public class DummyActionSO : AGameActionSO
    {
        protected override AAction<ActionContext, ActionTarget, ActionResult> CreateAction(object param) => new DummyAction();

        public override Type ParameterType => typeof(int);
    }
}