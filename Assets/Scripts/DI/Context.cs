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