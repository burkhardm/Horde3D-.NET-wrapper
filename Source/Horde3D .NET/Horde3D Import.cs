// *************************************************************************************************
//
// Horde3D .NET wrapper
// ----------------------------------
// Copyright (C) 2007 Martin Burkhard
//
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
// *************************************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Horde3DNET
{
    /// <summary>
    /// Separates native methods from managed code.
    /// </summary>
    internal static class NativeMethodsEngine
    {
        private const string ENGINE_DLL = "Horde3D.dll";

        // --- Basic funtions ---
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr getVersionString();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool init();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void release();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void resize(int x, int y, int width, int height);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void render();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void clear();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
    	internal static extern  bool loadPipelineConfig(string configFilename);

        // --- General functions ---
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr getMessage(out uint level, out float time);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint getActiveCamera();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setActiveCamera(uint camNode);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern float getOption(Horde3D.EngineOptions param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setOption(Horde3D.EngineOptions param, float value);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void showOverlay(float x_ll, float y_ll, float u_ll, float v_ll,
						                      float x_lr, float y_lr, float u_lr, float v_lr,
						                      float x_ur, float y_ur, float u_ur, float v_ur,
						                      float x_ul, float y_ul, float u_ul, float v_ul,
						                      uint layer, uint material);

        // --- Resource functions ---
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern Horde3D.ResourceTypes getResourceType(uint res);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
	    internal static extern uint findResource(Horde3D.ResourceTypes type, string name);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addResource(Horde3D.ResourceTypes type, string name, uint flags);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool removeResource(uint res);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool loadResource(string name, IntPtr data, uint size);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool unloadResource(uint res);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr getResourceData(uint res, Horde3D.ResourceData param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool updateResourceData(uint res, Horde3D.ResourceData param, IntPtr data, uint size);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr queryUnloadedResource();

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr getResourcePath(Horde3D.ResourceTypes type);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void setResourcePath(Horde3D.ResourceTypes type, string path);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern void releaseUnusedResources();

        // --- Material specific ---
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
	    internal static extern bool setMaterialUniform(uint matRes, string name, float a, float b, float c, float d);


        // --- Scene graph functions ---
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern Horde3D.SceneNodeTypes getNodeType(uint node);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr getNodeName(uint node);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setNodeName(uint node, string name);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint getNodeParent(uint node);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint getNodeChild(uint parent, string name, uint index, [MarshalAs(UnmanagedType.U1)]bool recursive);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addNodes(uint parent, uint res);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type
        internal static extern bool removeNode(uint node);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setNodeActivation(uint node, [MarshalAs(UnmanagedType.U1)]bool active);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool getNodeTransform(uint node, out float px, out float py, out float pz,
                                out float rx, out float ry, out float rz, out float sx, out float sy, out float sz);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setNodeTransform(uint node, float px, float py, float pz,
                                float rx, float ry, float rz, float sx, float sy, float sz);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]        
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool getNodeTransformMatrices(uint node, out IntPtr relMat, out IntPtr absMat);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setNodeTransformMatrix(uint node, float[] mat4x4);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool getNodeAABB(uint node, out float minX, out float minY, out float minZ, out float maxX, out float maxY, out float maxZ);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint castRay(uint node, float ox, float oy, float oz, float dx, float dy, float dz);


        // Group specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addGroupNode(uint parent, string name);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern float getGroupParam(uint node, Horde3D.GroupNodeParams param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setGroupParam(uint node, Horde3D.GroupNodeParams param, float value);

        // Model specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addModelNode(uint parent, string name, uint geoRes);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setupModelAnimStage(uint node, uint stage, uint res,
                                      string animMask, [MarshalAs(UnmanagedType.U1)]bool additive);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setModelAnimParams(uint node, uint stage, float time, float weight);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setModelMorpher(uint node, string target, float weight);

        // Mesh specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addMeshNode(uint parent, string name, uint matRes, 
								    uint batchStart, uint batchCount,
							    uint vertRStart, uint vertREnd );

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern float getMeshParam(uint node, Horde3D.MeshNodeParams param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setMeshParam(uint node, Horde3D.MeshNodeParams param, float value);

        [DllImport(ENGINE_DLL)]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool getMeshData(uint node, out uint vertPosArrayCount, out IntPtr vertPosArrayData,
                              out uint indexArrayCount, out IntPtr indexArrayData, out uint indexOffset);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setMeshUniform(uint node, string name, float a, float b, float c, float d, [MarshalAs(UnmanagedType.U1)]bool recursive);
        
        // Joint specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addJointNode(uint parent, string name, uint jointIndex);

        // Light specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addLightNode(uint parent, string name, uint materialRes,
                                     string lightingContext, string shadowContext);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern float getLightParam(uint node, Horde3D.LightNodeParams param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setLightParam(uint node, Horde3D.LightNodeParams param, float value);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setLightContexts( uint node, string lightingContext, string shadowContext );


        // Camera specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addCameraNode(uint parent, string name);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern float getCameraParam(uint node, Horde3D.CameraNodeParams param);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
        internal static extern bool setCameraParam(uint node, Horde3D.CameraNodeParams param, float value);

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return : MarshalAs(UnmanagedType.U1)] // represents C++ bool type 
        internal static extern bool setupCameraView(uint node, float fov, float aspect, float nearDist, float farDist);
            
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return : MarshalAs(UnmanagedType.U1)] // represents C++ bool type 
        internal static extern bool calcCameraProjectionMatrix(uint node, float[] projMat);


	    // Emitter specific
        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        internal static extern uint addEmitterNode( uint parent, string name,
								       uint matRes, uint effectRes,
								       uint maxParticleCount, int respawnCount );

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
	    internal static extern float getEmitterParam( uint node, Horde3D.EmitterNodeParams param );

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
	    internal static extern bool setEmitterParam( uint node, Horde3D.EmitterNodeParams param, float value );

        [DllImport(ENGINE_DLL), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.U1)]   // represents C++ bool type 
	    internal static extern bool advanceEmitterTime( uint node, float timeDelta );
    }
}