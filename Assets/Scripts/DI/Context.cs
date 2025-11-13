using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HSD.DI
{
    [DefaultExecutionOrder(-1000)]
    public class Context : MonoBehaviour
    {
        [SerializeField]
        List<Installer> _installers = new List<Installer>();
        [SerializeField]
        int count = 0;

        [Inject]
        public void Init(int _count)
        {
            count = _count;
        }

        protected virtual void Awake()
        {
            InstallInstallers();
        }

        protected void InstallInstallers()
        {
            foreach (Installer installer in _installers)
            {
                installer.Binding();
            }
        }
    }
}