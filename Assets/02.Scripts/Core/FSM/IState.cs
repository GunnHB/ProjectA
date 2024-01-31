using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace FSM
{
    public interface IState
    {
        /// <summary>
        /// 상태 진입
        /// </summary>
        void OperateEnter();

        /// <summary>
        /// 상태 지속
        /// </summary>
        void OperateUpdate();

        /// <summary>
        /// 상태 퇴출
        /// </summary>
        void OperateExit();
    }
}