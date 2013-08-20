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
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Horde3DNET.Properties;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
[assembly: CLSCompliant(true)]
namespace Horde3DNET
{
    public static class Horde3D
    {
        // Predefined constants
        private static int _rootNode = 1;
        private static int _primeTimeCam = 2;

        public static int RootNode
        {
            get { return _rootNode; }
        }

        public static int PrimeTimeCam
        {
            get { return _primeTimeCam; }
        }

        // Flags
        public enum ResourceFlags
        {
            NoQuery = 1,
            NoCompression = 2
        }

        public enum EngineOptions
        {
            TrilinearFiltering,
            AnisotropyFactor,
            TexCompression,
            LoadTextures,
            FastAnimation,
            OcclusionCulling,
            ShadowMapSize,
            DebugViewMode
        }

        public enum ResourceTypes
        {
            SceneGraph,
            Geometry,
            Animation,
            Material,
            Code,
            Shader,
            Texture2D,
            TextureCube,
            Effect,
            Undefined = 9999
        }

        public enum ResourceData
        {
            AnimFrameCount,
            Tex2DPixelData
        }

        public enum SceneNodeTypes
        {
            Group,
            Model,
            Mesh,
            Joint,
            Light,
            Camera,
            Emitter,
            Undefined = 9999
        }

        public enum GroupNodeParams
        {
            MinDist,
            MaxDist
        }

        public enum MeshNodeParams
        {
            MaterialRes
        }

        public enum LightNodeParams
        {
            MaterialRes,
            Radius,
            FOV,
            Col_R,
            Col_G,
            Col_B,
            ShadowMapCount,
            ShadowSplitLambda,
            ShadowMapBias
        }

        public enum CameraNodeParams
        {
            LeftPlane,
            RightPlane,
            BottomPlane,
            TopPlane,
            NearPlane,
            FarPlane
        }

        public enum EmitterNodeParams
        {
            Delay,
            EmissionRate,
            SpreadAngle,
            ForceX,
            ForceY,
            ForceZ
        }

        // --- Basic funtions ---
        /// <summary>
        /// This function returns a string containing the current version of Horde3D.
        /// </summary>
        /// <returns>The version string</returns>
        public static string getVersionString()
        {
            IntPtr ptr = NativeMethodsEngine.getVersionString();
            return Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// This function initializes the graphics engine and makes it ready for use. 
        /// It has to be the first call to the engine except for getVersionString.
        /// </summary>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool init()
        {
            if (getVersionString() != Resources.VersionString)
                throw new LibraryIncompatibleException(Resources.LibraryIncompatibleExceptionString);

            return NativeMethodsEngine.init();
        }

        /// <summary>
        /// This function releases the engine and frees all objects and associated memory. 
        /// It should be called when the application is destroyed.
        /// </summary>
        public static void release()
        {
            NativeMethodsEngine.release();
        }

        /// <summary>
        /// This function sets the dimensions of the rendering viewport. 
        /// It has to be called after initialization and whenever the viewport size changes.
        /// </summary>
        /// <param name="x">the x-position of the viewport in the rendering context</param>
        /// <param name="y">the y-position of the viewport in the rendering context</param>
        /// <param name="width">the width of the viewport</param>
        /// <param name="height">the height of the viewport</param>
        public static void resize(int x, int y, int width, int height)
        {
            NativeMethodsEngine.resize(x, y, width, height);
        }

        /// <summary>
        /// This is the main function of the engine. 
        /// It executes all the rendering, animation and other tasks. 
        /// This function is usually called once per frame.
        /// </summary>
        public static void render()
        {
            NativeMethodsEngine.render();
        }

        /// <summary>
        /// This function removes all nodes from the scene graph except the root node and releases all resources.
        /// Warning: All resource and node IDs are invalid after calling this function. 
        /// This method might affect the engine stability.
        /// </summary>
        /// <remarks>
        /// clear() method is not stable. Clearing the scene and adding new scene content might cause an exception.
        /// </remarks>
        public static void clear()
        {
            NativeMethodsEngine.clear();
        }

        /// <summary>
        /// This function loads the pipeline configuration from a specified file. If another configuration has already been loaded the pipeline is reset and initialized with the new configuration.
        /// </summary>
        /// <param name="configFilename">name of the pipeline configuration file to be loaded</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool loadPipelineConfig(string configFilename)
        {
            if (configFilename == null) throw new ArgumentNullException("configFilename", Resources.StringNullExceptionString);
            if (!File.Exists(configFilename)) return false;

            return NativeMethodsEngine.loadPipelineConfig(configFilename);
        }

        // --- General functions ---

        /// <summary>
        /// This function returns the next message string from the message queue and writes additional information to the specified variables. If no message is left over in the queue an empty string is returned.
        /// </summary>
        /// <param name="level">pointer to variable for storing message level indicating importance (can be NULL)</param>
        /// <param name="time">pointer to variable for stroing time when message was added (can be NULL)</param>
        /// <returns>message string or empty string if no message is in queue</returns>
        public static string getMessage(out uint level, out float time)
        {
            IntPtr ptr = NativeMethodsEngine.getMessage(out level, out time);
            return Marshal.PtrToStringAnsi(ptr);
        }


        /// <summary>
        /// This function returns the handle to the currently actice camera node from which the scene is rendered.
        /// </summary>
        /// <returns>handle to the active camera node</returns>
        public static int getActiveCamera()
        {
            return (int)NativeMethodsEngine.getActiveCamera();
        }

        /// <summary>
        /// his function sets the specified camera node as active camera from which the scene is rendered.
        /// </summary>
        /// <param name="camNode">handle to camera node</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setActiveCamera(int camNode)
        {
            if (camNode < 0) throw new ArgumentOutOfRangeException("camNode", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setActiveCamera((uint)camNode);
        }

        /// <summary>
        /// This function gets a specified option parameter and returns its value.
        /// </summary>
        /// <param name="param">option parameter</param>
        /// <returns>current value of the specified option parameter</returns>
        public static float getOption(EngineOptions param)
        {
            return NativeMethodsEngine.getOption(param);
        }

        /// <summary>
        /// This function sets a specified option parameter to a specified value.
        /// </summary>
        /// <param name="param">option parameter</param>
        /// <param name="value">value of the option parameter</param>
        /// <returns>true if the option could be set to the specified value, otherwise false</returns>
        public static bool setOption(EngineOptions param, float value)
        {
            return NativeMethodsEngine.setOption(param, value);
        }

        /// <summary>
        /// This function displays an overlay with a specified material at a specified position on the screen.
        /// </summary>
        /// <remarks>
        /// An overlay is a 2D image that can be used to render 2D GUI elements. 
        /// The coordinate system used has its origin (0, 0) at the lower left corner of the screen and its maximum (1, 1) at the upper right corner. 
        /// Texture coordinates are using the same system, where the coordinates (0, 0) correspond to the lower left corner of the image. 
        /// Overlays can have different layers which describe the order in which they are drawn. 
        /// Overlays with smaller layer numbers are drawn before overlays with higher layer numbers.
        /// </remarks>
        /// <param name="x_ll">x position of the lower left corner</param>
        /// <param name="y_ll">y position of the lower left corner</param>
        /// <param name="u_ll">u texture coordinate of the lower left corner</param>
        /// <param name="v_ll">v texture coordinate of the lower left corner</param>
        /// <param name="x_lr">x position of the lower right corner</param>
        /// <param name="y_lr">y position of the lower right corner</param>
        /// <param name="u_lr">u texture coordinate of the lower right corner</param>
        /// <param name="v_lr">v texture coordinate of the lower right corner</param>
        /// <param name="x_ur">x position of the upper right corner</param>
        /// <param name="y_ur">y position of the upper right corner</param>
        /// <param name="u_ur">u texture coordinate of the upper right corner</param>
        /// <param name="v_ur">v texture coordinate of the upper right corner</param>
        /// <param name="x_ul">x position of the upper left corner</param>
        /// <param name="y_ul">y position of the upper left corner</param>
        /// <param name="u_ul">u texture coordinate of the upper left corner</param>
        /// <param name="v_ul">v texture coordinate of the upper left corner</param>
        /// <param name="layer">layer index of the overlay (Values: from 0 to 7)</param>
        /// <param name="material">material resource used for rendering</param>
        public static void showOverlay(float x_ll, float y_ll, float u_ll, float v_ll,
                                        float x_lr, float y_lr, float u_lr, float v_lr,
                                        float x_ur, float y_ur, float u_ur, float v_ur,
                                        float x_ul, float y_ul, float u_ul, float v_ul,
                                        int layer, int material)
        {
            if (layer < 0) throw new ArgumentOutOfRangeException("layer", Resources.UIntOutOfRangeExceptionString);
            if (material < 0) throw new ArgumentOutOfRangeException("material", Resources.UIntOutOfRangeExceptionString);

            NativeMethodsEngine.showOverlay(x_ll, y_ll, u_ll, v_ll, x_lr, y_lr, u_lr, v_lr, x_ur, y_ur, u_ur, v_ur, x_ul, y_ul, u_ul, v_ul, (uint)layer, (uint)material);
        }


        // --- Resource functions ---
        /// <summary>
        /// This function returns the type of a specified resource. 
        /// If the resource handle is invalid, the function returns the resource type 'Unknown'.
        /// </summary>
        /// <param name="res">handle to the resource whose type will be returned</param>
        /// <returns>type of the scene node</returns>
        public static ResourceTypes getResourceType(int res)
        {
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getResourceType((uint)res);
        }

        /// <summary>
        /// This function searches the resource of the specified type and name and returns its handle. If the resource is not available in the resource manager a zero handle is returned.
        /// </summary>
        /// <remarks>
        /// The content path of the specified ResourceType is added automatically.
        /// </remarks>
        /// <param name="type">type of the resource</param>
        /// <param name="name">name of the resource</param>
        /// <returns>handle to the resource or 0 if not found</returns>
        public static int findResource(ResourceTypes type, string name)
        {
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            return (int)NativeMethodsEngine.findResource(type, name);
        }

        /// <summary>
        /// This function tries to add a resource of a specified type and name to the resource manager. 
        /// If a resource of the same type and name is already found, the handle to the existing resource is returned instead of creating a new one.
        /// </summary>
        /// <param name="type">type of the resource</param>
        /// <param name="name">name of the resource</param>
        /// <param name="flags">flags used for creating the resource</param>
        /// <returns>handle to the resource to be added or 0 in case of failure</returns>
        public static int addResource(ResourceTypes type, string name, int flags)
        {
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (flags < 0) throw new ArgumentOutOfRangeException("flags", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addResource(type, name, (uint)flags);
        }

        /// <summary>
        /// This function removes a specified resource from the resource manager. 
        /// If the resource is still in use, this operation is delayed.
        /// Note: The memory is not freed until you call releaseUnusedResources.
        /// </summary>
        /// <param name="res">handle to the resource to be removed</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool removeResource(int res)
        {
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.removeResource((uint)res);
        }

        /// <summary>
        /// This function loads data for a resource that was previously added to the resource manager.
        /// </summary>
        /// <remarks>
        /// If data is null the resource manager is told that the resource doesn't have any data. 
        /// The function can only be called once for every resource.
        /// Important Note: The data block must be null terminated!
        /// </remarks>
        /// <param name="name">name of the resource for which the data is loaded</param>
        /// <param name="data">the data to be loaded (null terminated block)</param>
        /// <param name="size">size of the data block</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool loadResource(string name, byte[] data, int size)
        {
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (data == null) throw new ArgumentNullException("data");

            if (size < 0) throw new ArgumentOutOfRangeException("size", Resources.UIntOutOfRangeExceptionString);

            if (data.Length < size)
                throw new ArgumentException(Resources.LoadResourceArgumentExceptionString, "data");

            // allocate memory for resource data
            IntPtr ptr = Marshal.AllocHGlobal(size + 1);

            // copy byte data into allocated memory
            Marshal.Copy(data, 0, ptr, size);

            // terminate data block
            Marshal.WriteByte(ptr, size, 0x00);

            // load resource
            bool result = NativeMethodsEngine.loadResource(name, ptr, (uint)size);

            // free previously allocated memory
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        /// <summary>
        /// This function unloads a previously loaded resource and restores the default values it had before loading. The state is set back to unloaded which makes it possible to load the resource again.
        /// </summary>
        /// <param name="res">handle to resource to be unloaded</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool unloadResource(int res)
        {
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.unloadResource((uint) res);
        }

        /// <summary>
        /// This function returns a pointer to the specified data of a specified resource. 
        /// For information on the format (uint, float, ..) of the pointer see the ResourceData description. 
        /// </summary>
        /// <param name="res">handle to the resource to be accessed</param>
        /// <param name="param">parameter indicating data of the resource that will be accessed</param>
        /// <returns>specified resource data if it is available</returns>
        public static IntPtr getResourceData(int res, ResourceData param)
        {
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getResourceData((uint)res, param);
        }

        /// <summary>
        /// This function updates the content of a resource that was successfully loaded before.
        /// The new data must have exactly the same data layout as the data that was loaded.
        /// </summary>
        /// <remarks>Notes on available ResourceData parameters: 
        /// Tex2DPixelData - Sets the image data of a Texture2D resource. 
        /// The data must point to a memory block that contains the pixels of the image. 
        /// Each pixel needs to have 32 bit color data in BGRA format. 
        /// The dimensions of the image (width, height) must be exactly the same as the dimensions 
        /// of the image that was originally loaded for the resource.
        /// PLEASE NOTE: calls to updateResourceData are not threadsafe and so it might not work in case you have a separated render thread.</remarks>
        /// <param name="res">handle to the resource for which the data is modified</param>
        /// <param name="param">data structure which will be updated</param>
        /// <param name="data">the new data</param>
        /// <param name="size">size of the new data block</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool updateResourceData(int res, ResourceData param, byte[] data, int size)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);
            if (size < 0) throw new ArgumentOutOfRangeException("size", Resources.UIntOutOfRangeExceptionString);

            if (data.Length < size)
                throw new ArgumentException(Resources.LoadResourceArgumentExceptionString, "data");

            // allocate memory for resource data
            IntPtr ptr = Marshal.AllocHGlobal(size + 1);

            // copy byte data into allocated memory
            Marshal.Copy(data, 0, ptr, size);

            // terminate string
            Marshal.WriteByte(ptr, size, 0x00);

            // load resource
            bool result = NativeMethodsEngine.updateResourceData((uint)res, param, ptr, (uint)size);

            // free previously allocated memory
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        /// <summary>
        /// This function searches a resource that is not yet loaded and returns the resource name string. 
        /// If there are no unloaded resources, an empty string is returned.
        /// </summary>
        /// <returns>name of an unloaded resource or empty string</returns>
        public static string queryUnloadedResource()
        {
            IntPtr ptr = NativeMethodsEngine.queryUnloadedResource();
            return Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// This function returns the search path of a specified resource type.
        /// </summary>
        /// <param name="type">type of resource</param>
        /// <returns>the search path string</returns>
        public static string getResourcePath(ResourceTypes type)
        {
            IntPtr ptr = NativeMethodsEngine.getResourcePath(type);
            return Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// This function sets the search path for a specified resource type. 
        /// Whenever a new resource is added, the specified path is concatenated to the name of the created resource.
        /// </summary>
        /// <param name="type">type of resource</param>
        /// <param name="path">path where the resources can be found (without slash or backslash at the end)</param>
        public static void setResourcePath(ResourceTypes type, string path)
        {
            if (path == null) throw new ArgumentNullException("path", Resources.StringNullExceptionString);

            NativeMethodsEngine.setResourcePath(type, path);
        }

        /// <summary>
        /// This function releases resources that are no longer used. 
        /// Unused resources were either told to be released by the user calling removeResource() or are no more referenced by any other engine objects.
        /// </summary>
        public static void releaseUnusedResources()
        {
            NativeMethodsEngine.releaseUnusedResources();
        }


        // Material specific
        /// <summary>
        /// This function sets the specified shader uniform of the specified material to the specified values.
        /// </summary>
        /// <param name="matRes">handle to the Material resource to be accessed</param>
        /// <param name="name">name of the uniform as defined in Material resource</param>
        /// <param name="a">value of first component</param>
        /// <param name="b">value of second component</param>
        /// <param name="c">value of third component</param>
        /// <param name="d">value of fourth component</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setMaterialUniform(int matRes, string name, float a, float b, float c, float d)
        {
            if (matRes < 0) throw new ArgumentOutOfRangeException("matRes", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setMaterialUniform((uint)matRes, name, a, b, c, d);
        }


        // SceneGraph functions
        /// <summary>
        /// This function returns the type of a specified scene node. 
        /// If the node handle is invalid, the function returns the node type 'Unknown'.
        /// </summary>
        /// <param name="node">handle to the scene node whose type will be returned</param>
        /// <returns>type of the scene node</returns>
        public static SceneNodeTypes getNodeType(int node)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getNodeType((uint)node);
        }

        /// <summary>
        /// This function returns the name of a specified scene node. 
        /// If the node handle is invalid, the function returns an empty string.
        /// </summary>
        /// <param name="node">handle to the scene node whose name will be returned</param>
        /// <returns>name of the scene node or empty string in case of failure</returns>
        public static string getNodeName(int node)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            IntPtr ptr = NativeMethodsEngine.getNodeName((uint)node);
            return Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// This function sets the name of the specified scene node to the specified value.
        /// </summary>
        /// <param name="node">handle to the scene node whose name will be changed</param>
        /// <param name="name">new name of the scene node</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setNodeName(int node, string name)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setNodeName((uint)node, name);
        }

        /// <summary>
        /// his function returns the handle to the parent node of a specified scene node. If the specified node handle is invalid or the root node, 0 is returned.
        /// </summary>
        /// <param name="node">handle to the scene node whose parent will be returned</param>
        /// <returns>handle to parent node or 0 in case of failure</returns>
        public static int getNodeParent(int node)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.getNodeParent((uint)node);
        }

        /// <summary>
        /// This function looks for the n-th child node with a specified name and returns its handle. 
        /// If no matching child node was found, the function returns 0.
        /// </summary>
        /// <param name="parent">handle to the parent node</param>
        /// <param name="name">name of the child node; if name is empty string, all children are considered</param>
        /// <param name="index">index of the child node (useful if there are several matching children)</param>
        /// <param name="recursive">specifies whether the node is scanned recursively, meaning that children of a child are also examined</param>
        /// <returns>the handle to the child node or 0 if no matching child was found</returns>
        public static int getNodeChild(int parent, string name, int index, bool recursive)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);            
            if (index < 0) throw new ArgumentOutOfRangeException("index", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.getNodeChild((uint)parent, name, (uint)index, recursive);
        }

        /// <summary>
        /// This function creates several new nodes as described in a SceneGraph resource and attaches them to a specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the root of the new nodes will be attached</param>
        /// <param name="res">handle to the SceneGraph resource</param>
        /// <returns>handle to the root of the created nodes or 0 in case of failure</returns>
        public static int addNodes(int parent, int res)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addNodes((uint)parent, (uint)res);
        }

        /// <summary>
        /// This function removes the specified node and all of it's children from the scene.
        /// </summary>
        /// <param name="node">handle to the node to be removed</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool removeNode(int node)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.removeNode((uint)node);
        }

        /// <summary>
        /// This function sets the activation state of the specified node to active or inactive. Inactive nodes are excluded from rendering.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="active">boolean value indicating whether node is active or inactive</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setNodeActivation(int node, bool active)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setNodeActivation((uint)node, active);
        }

        /// <summary>
        /// This function gets the translation, rotation and scale of a specified scene node object.
        /// The coordinates are in local space and contain the transformation of the node relative to its parent.
        /// </summary>
        /// <param name="node">handle to the node which will be accessed</param>
        /// <param name="px">x variable where position of the node will be stored</param>
        /// <param name="py">y variable where position of the node will be stored</param>
        /// <param name="pz">z variable where position of the node will be stored</param>
        /// <param name="rx">x variable where rotation of the node in Euler angles (degrees) will be stored</param>
        /// <param name="ry">y variable where rotation of the node in Euler angles (degrees) will be stored</param>
        /// <param name="rz">z variable where rotation of the node in Euler angles (degrees) will be stored</param>
        /// <param name="sx">x variable where scale of the node will be stored</param>
        /// <param name="sy">y variable where scale of the node will be stored</param>
        /// <param name="sz">z variable where scale of the node will be stored</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool getNodeTransform(int node, out float px, out float py, out float pz,
                                out float rx, out float ry, out float rz, out float sx, out float sy, out float sz)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getNodeTransform((uint)node, out px, out py, out pz, out rx, out ry, out rz, out sx, out sy, out sz);
        }

        /// <summary>
        /// This function sets the relative translation, rotation and scale of a specified scene node object.
        /// The coordinates are in local space and contain the transformation of the node relative to its parent.
        /// </summary>
        /// <param name="node">handle to the node which will be modified</param>
        /// <param name="px">x position of the node</param>
        /// <param name="py">y position of the node</param>
        /// <param name="pz">z position of the node</param>
        /// <param name="rx">x rotation of the node in Euler angles (degrees)</param>
        /// <param name="ry">y rotation of the node in Euler angles (degrees)</param>
        /// <param name="rz">z rotation of the node in Euler angles (degrees)</param>
        /// <param name="sx">x scale of the node</param>
        /// <param name="sy">y scale of the node</param>
        /// <param name="sz">z scale of the node</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setNodeTransform(int node, float px, float py, float pz,
                                float rx, float ry, float rz, float sx, float sy, float sz)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setNodeTransform((uint)node, px, py, pz, rx, ry, rz, sx, sy, sz);
        }

        /// <summary>
        /// This function returns a pointer to the relative and absolute transformation matrices of the specified node in the specified pointer variables.
        /// </summary>
        /// <param name="node">handle to the scene node whose matrices will be accessed</param>
        /// <param name="relMat">pointer to a variable where the address of the relative transformation matrix will be stored</param>
        /// <param name="absMat">pointer to a variable where the address of the absolute transformation matrix will be stored</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool getNodeTransformMatrices(int node, out IntPtr relMat, out IntPtr absMat)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getNodeTransformMatrices((uint)node, out relMat, out absMat);
        }

        /// <summary>
        /// This function sets the relative transformation matrix of the specified scene node. 
        /// It is basically the same as setNodeTransform but takes directly a matrix instead of individual transformation parameters.
        /// </summary>
        /// <param name="node">handle to the scene node whose matrix will be updated</param>
        /// <param name="mat4x4">array of a 4x4 matrix in column major order</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setNodeTransformMatrix(int node, float[] mat4x4)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (mat4x4.Length != 16) throw new ArgumentOutOfRangeException("mat4x4", Resources.MatrixOutOfRangeExceptionString);

            return NativeMethodsEngine.setNodeTransformMatrix((uint)node, mat4x4);
        }

        /// <summary>
        /// This function stores the world coordinates of the axis aligned bounding box of a specified node in the specified variables. 
        /// The bounding box is represented using the minimum and maximum coordinates on all three axes. 
        /// </summary>
        /// <param name="node">handle to the node which will be accessed</param>
        /// <param name="minX">variable where minimum x-coordinates will be stored</param>
        /// <param name="minY">variable where minimum y-coordinates will be stored</param>
        /// <param name="minZ">variable where minimum z-coordinates will be stored</param>
        /// <param name="maxX">variable where maximum x-coordinates will be stored</param>
        /// <param name="maxY">variable where maximum y-coordinates will be stored</param>
        /// <param name="maxZ">variable where maximum z-coordinates will be stored</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool getNodeAABB(int node, out float minX, out float minY, out float minZ, out float maxX, out float maxY, out float maxZ)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getNodeAABB((uint)node, out minX, out minY, out minZ, out maxX, out maxY, out maxZ);
        }

        /// <summary>
        /// This function checks recursively if the specified ray intersects the specified node or one of its children. 
        /// The function finds the nearest intersection relative to the ray origin and returns the handle to the corresponding scene node. 
        /// The ray is a line segment and is specified by a starting point (the origin) and a finite direction vector which also defines its length. 
        /// Currently this function is limited to returning intersections with Meshes.
        /// </summary>
        /// <param name="node">node at which intersection check is beginning</param>
        /// <param name="ox">ray origin</param>
        /// <param name="oy">ray origin</param>
        /// <param name="oz">ray origin</param>
        /// <param name="dx">ray direction vector also specifying ray length</param>
        /// <param name="dy">ray direction vector also specifying ray length</param>
        /// <param name="dz">ray direction vector also specifying ray length</param>
        /// <returns>handle to nearest intersected node or 0 if no node was hit</returns>
        public static uint castRay(int node, float ox, float oy, float oz, float dx, float dy, float dz)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.castRay((uint)node, ox, oy, oz, dx, dy, dz);
        }



        // Group specific
        /// <summary>
        /// This function creates a new Group node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addGroupNode(int parent, string name)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);

            return (int)NativeMethodsEngine.addGroupNode((uint)parent, name);
        }

        /// <summary>
        /// This function returns a specified property of the specified node. The specified node must be a Group node.
        /// </summary>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="param">parameter to be accessed</param>
        /// <returns>value of the parameter</returns>
        public static float getGroupParam(int node, GroupNodeParams param)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getGroupParam((uint)node, param);
        }

        /// <summary>
        /// This function sets a specified property of the specified node to a specified value. 
        /// The specified node must be a Group node.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="param">parameter to be modified</param>
        /// <param name="value">new value for the specified parameter</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool setGroupParam(int node, GroupNodeParams param, float value)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setGroupParam((uint)node, param, value);
        }


        // Model specific
        /// <summary>
        /// This function creates a new Model node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <param name="geoRes">Geometry resource used by Model node</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addModelNode(int parent, string name, int geoRes)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (geoRes < 0) throw new ArgumentOutOfRangeException("geoRes", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addModelNode((uint)parent, name, (uint)geoRes);
        }

        /// <summary>
        /// This function is used to setup the specified animation stage (channel) of the specified Model node.
        /// </summary>
        /// <remarks>
        /// The function operates on Model nodes but accepts also Group nodes in which case the call is passed recursively to the Model child nodes.
        /// The function is used for animation blending. There is a fixed number of stages (by default 16) on which different animations can be played. 
        /// The animation mask determines which child-nodes of the model (joints or meshes) are affected by the specified animation on the stage to be configured. 
        /// If the mask is an empty string, the animation affects all nodes. Otherwise the mask can contain several node names separated by the two character sequence. 
        /// When a mask is specified, the initial state of all nodes is 'not affected by animation'. For every node in the mask the function recurses down the (skeleton-) 
        /// hierarchy starting at the currently processed node in the mask and inverts the state of the considered nodes. This makes it possible to do complex animation mixing.
        /// A simpler way to do animation mixing is using additive animations. If a stage is configured to be additive the engine calculates the difference between the current 
        /// frame and the first frame in the animation and adds this delta to the current transformation of the joints or meshes.
        /// </remarks>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="stage">index of the animation stage to be configured</param>
        /// <param name="res">handle to Animation resource</param>
        /// <param name="animMask">mask defining which nodes will be affected by animation</param>
        /// <param name="additive">flag indicating whether stage is additive</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setupModelAnimStage(int node, int stage, int res, string animMask, bool additive)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (stage < 0) throw new ArgumentOutOfRangeException("stage", Resources.UIntOutOfRangeExceptionString);
            if (res < 0) throw new ArgumentOutOfRangeException("res", Resources.UIntOutOfRangeExceptionString);
            if (animMask == null) throw new ArgumentNullException("animMask", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setupModelAnimStage((uint)node, (uint)stage, (uint)res, animMask, additive);
        }

        /// <summary>
        /// This function sets the current animation time and weight for a specified stage of the specified model.</summary>
        /// <remarks>
        /// The time corresponds to the frames of the animation and the animation is looped if the time is higher than the maximum number of frames in the Animation resource. 
        /// The weight is used for animation blending and determines how much influence the stage has compared to the other active stages. 
        /// When the sum of the weights of all stages is more than one, the animations on the lower stages get priority. 
        /// The function operates on Model nodes but accepts also Group nodes in which case the call is passed recursively to the Model child nodes.
        /// </remarks>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="stage">index of the animation stage to be modified</param>
        /// <param name="time">new animation time</param>
        /// <param name="weight">new animation weight</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setModelAnimParams(int node, int stage, float time, float weight)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (stage < 0) throw new ArgumentOutOfRangeException("stage", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setModelAnimParams((uint)node, (uint)stage, time, weight);
        }

        /// <summary>
        /// This function sets the weight of a specified morph target. If the target parameter is an empty string the weight of all morph targets in the specified Model node is modified. The function operates on Model nodes but accepts also Group nodes in which case the call is passed recursively to the Model child nodes. If the specified morph target is not found the function returns false.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="target">name of morph target</param>
        /// <param name="weight">new weight for morph target</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setModelMorpher(int node, string target, float weight)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (target == null) throw new ArgumentNullException("target", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setModelMorpher((uint)node, target, weight);
        }

        // Mesh specific
        /// <summary>
        /// This function creates a new Mesh node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <param name="matRes">Material resource used by Mesh node</param>
        /// <param name="batchStart">first vertex index in Geometry resource of parent Model node</param>
        /// <param name="batchCount">number of vertex indices in Geometry resource of parent Model node</param>
        /// <param name="vertRStart">minimum vertex array index contained in Geometry resource indices of parent Model node</param>
        /// <param name="vertREnd">maximum vertex array index contained in Geometry resource indices of parent Model node</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addMeshNode(int parent, string name, int matRes, int batchStart, int batchCount, int vertRStart, int vertREnd)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (matRes < 0) throw new ArgumentOutOfRangeException("matRes", Resources.UIntOutOfRangeExceptionString);
            if (batchStart < 0) throw new ArgumentOutOfRangeException("batchStart", Resources.UIntOutOfRangeExceptionString);
            if (batchCount < 0) throw new ArgumentOutOfRangeException("batchCount", Resources.UIntOutOfRangeExceptionString);
            if (vertRStart < 0) throw new ArgumentOutOfRangeException("vertRStart", Resources.UIntOutOfRangeExceptionString);
            if (vertREnd < 0) throw new ArgumentOutOfRangeException("vertREnd", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addMeshNode((uint)parent, name, (uint)matRes, (uint)batchStart, (uint)batchCount, (uint)vertRStart, (uint)vertREnd);
        }

        /// <summary>
        /// This function returns a specified property of the specified node. The specified node must be a Mesh node.
        /// </summary>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="param">parameter to be accessed</param>
        /// <returns>value of the parameter</returns>
        public static float getMeshParam(int node, MeshNodeParams param)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getMeshParam((uint)node, param);
        }

        /// <summary>
        /// This function sets a specified property of the specified node to a specified value. The specified node must be a Mesh node, or alternatively a Model or Group node. Calls on the latter two node types are recursively passed to all Mesh child nodes.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="param">parameter to be modified</param>
        /// <param name="value">new value for the specified parameter</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool setMeshParam(int node, MeshNodeParams param, float value)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setMeshParam((uint)node, param, value);
        }

        /// <summary>
        /// This function gives access to the vertex position data and triangle vertex indices of the meshes geometry. 
        /// The vertex positions are untransformed and are stored as float coordinates in X, Y, Z order.
        /// </summary>
        /// <remarks>
        /// Important Note: The pointer is const and allows only read access to the data. 
        /// Do never try to modify the data of the pointer since that can corrupt the engine's internal states!
        /// </remarks>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="vertPosArrayCount">variable where number of vertices in position array will be stored</param>
        /// <param name="vertPosArrayData">vertex position data array</param>
        /// <param name="indexArrayCount">variable where number of indices will be stored</param>
        /// <param name="indexArrayData">index data array</param>
        /// <param name="indexOffset">variable where offset of the first index value relative to zero will be stored; required to access vertPosArrayData array with indexArayData indices</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool getMeshData(int node, out int vertPosArrayCount, out float[] vertPosArrayData, out int indexArrayCount, out int[] indexArrayData, out int indexOffset)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            uint _vertPosArrayCount, _indexArrayCount, _indexOffset;

            // pointers to access data in memory
            IntPtr vertPosArrayPointer;
            IntPtr indexArrayPointer;
            
            // invoke dll method
            if (NativeMethodsEngine.getMeshData((uint)node, out _vertPosArrayCount, out vertPosArrayPointer, out _indexArrayCount, out indexArrayPointer, out _indexOffset))
            {
                vertPosArrayCount = (int) _vertPosArrayCount;
                indexArrayCount = (int) _indexArrayCount;
                indexOffset = (int) _indexOffset;

                // dynamically create data arrays
                vertPosArrayData = new float[vertPosArrayCount];
                indexArrayData = new int[indexArrayCount];

                // copy data from memory to data array
                Marshal.Copy(vertPosArrayPointer, vertPosArrayData, 0, vertPosArrayCount);

                // copy data from memory to data array
                Marshal.Copy(indexArrayPointer, indexArrayData, 0, indexArrayCount);

                return true;
            }

            // no data available
            vertPosArrayCount = 0;
            vertPosArrayData = null;
            indexArrayCount = 0;
            indexArrayData = null;
            indexOffset = 0;

            return false;
        }

        /// <summary>
        /// This function sets the specified shader uniform of the specified mesh to the specified values. 
        /// Each mesh has its own copy of the uniforms defined in the associated Material resource and if the uniform has not explicitely been set using this function, the default values from the Material resource are applied for rendering.
        /// </summary>
        /// <param name="node">handle to the Mesh node to be accessed</param>
        /// <param name="name">name of the uniform as defined in mesh material resource</param>
        /// <param name="a">value of first component</param>
        /// <param name="b">value of second component</param>
        /// <param name="c">value of third component</param>
        /// <param name="d">value of fourth component</param>
        /// <returns>true in case of success otherwise false</returns>
        /// <param name="recursive">parameter specifying whether uniform is set recursively for all Mesh children of Mesh node</param>
        public static bool setMeshUniform(int node, string name, float a, float b, float c, float d, bool recursive)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setMeshUniform((uint)node, name, a, b, c, d, recursive);
        }

        // Joint specific
        /// <summary>
        /// This function creates a new Joint node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <param name="jointIndex">index of joint in Geometry resource of parent Model node</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addJointNode(int parent, string name, int jointIndex)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (jointIndex < 0) throw new ArgumentOutOfRangeException("jointIndex", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addJointNode((uint)parent, name, (uint)jointIndex);
        }

        // Light specific
        /// <summary>
        /// This function creates a new Light node and attaches it to the specified parent node. The direction vector of the untransformed light node is pointing along the the negative z-axis. The specified material resource can define uniforms and projective textures. Furthermore it can contain a shader for doing lighting calculations if deferred shading is used. If no material is required the parameter can be zero. The context names define which shader contexts are used when rendering shadow maps or doing light calculations for forward rendering configurations.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <param name="materialRes">material resource for light configuration or 0 if not used</param>
        /// <param name="lightingContext">name of the shader context used for doing light calculations</param>
        /// <param name="shadowContext">name of the shader context used for doing shadow map rendering</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addLightNode(int parent, string name, int materialRes, string lightingContext, string shadowContext)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (materialRes < 0) throw new ArgumentOutOfRangeException("materialRes", Resources.UIntOutOfRangeExceptionString);


            return (int)NativeMethodsEngine.addLightNode((uint)parent, name, (uint)materialRes, lightingContext, shadowContext);
        }

        /// <summary>
        /// This function returns a specified property of the specified node. The specified node must be a Light node.
        /// </summary>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="param">parameter to be accessed</param>
        /// <returns>value of the parameter</returns>
        public static float getLightParam(int node, LightNodeParams param)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getLightParam((uint)node, param);
        }

        /// <summary>
        /// This function sets a specified property of the specified node to a specified value. 
        /// The specified node must be a Light node, or alternatively a Group node. 
        /// Calls on the latter node type are recursively passed to all Light child nodes.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="param">parameter to be modified</param>
        /// <param name="value">new value for the specified parameter</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool setLightParam(int node, LightNodeParams param, float value)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setLightParam((uint)node, param, value);
        }


        /// <summary>
        /// This function sets the lighting and shadow shader contexts of the specified light source. 
        /// The contexts define which shader code is used when doing lighting calculations or rendering the shadow map. 
        /// The function works on a Light node or a Group node in which case the call is recursively passed to all child Light nodes.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="lightingContext">name of the shader context used for performing lighting calculations</param>
        /// <param name="shadowContext"><name of the shader context used for rendering shadow maps/param>
        /// <returns>true in case of success otherwise false</returns>
        public static  bool setLightContexts( int node, string lightingContext, string shadowContext )
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (lightingContext == null) throw new ArgumentNullException("lightingContext", Resources.StringNullExceptionString);
            if (shadowContext == null) throw new ArgumentNullException("shadowContext", Resources.StringNullExceptionString);

            return NativeMethodsEngine.setLightContexts((uint) node, lightingContext, shadowContext);
        }


        // Camera specific
        /// <summary>
        /// This function creates a new Camera node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addCameraNode(int parent, string name)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);

            return (int)NativeMethodsEngine.addCameraNode((uint)parent, name);
        }

        /// <summary>
        /// This function returns a specified property of the specified node. The specified node must be a Camera node.
        /// </summary>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="param">parameter to be accessed</param>
        /// <returns>value of the parameter</returns>
        public static float getCameraParam(int node, CameraNodeParams param)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getCameraParam((uint)node, param);
        }

        /// <summary>
        /// This function sets a specified property of the specified node to a specified value. The specified node must be a Camera node, or alternatively a Group node. Calls on the latter node type are recursively passed to all Camera child nodes.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="param">parameter to be modified</param>
        /// <param name="value">new value for the specified parameter</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool setCameraParam(int node, CameraNodeParams param, float value)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setCameraParam((uint)node, param, value);
        }


        /// <summary>
        /// This function calculates the view frustum planes of the specified camera node using the specified view parameters.
        /// </summary>
        /// <param name="node">handle to the Camera node which will be modified</param>
        /// <param name="fov">field of view (FOV) in degrees</param>
        /// <param name="aspect">aspect ratio</param>
        /// <param name="nearDist">distance of near clipping plane</param>
        /// <param name="farDist">distance of far clipping plane</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool setupCameraView(int node, float fov, float aspect, float nearDist, float farDist)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setupCameraView((uint) node, fov, aspect, nearDist, farDist);
        }

        /// <summary>
        /// This function calculates the camera projection matrix used for bringing the geometry to screen space and copies it to the specified array.
        /// </summary>
        /// <param name="node">handle to Camera node</param>
        /// <param name="projMat">pointer to float array with 16 elements</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool calcCameraProjectionMatrix(int node, float[] projMat)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);
            if (projMat.Length != 16) throw new ArgumentOutOfRangeException("projMat", Resources.MatrixOutOfRangeExceptionString);

            return NativeMethodsEngine.calcCameraProjectionMatrix((uint) node, projMat);
        }



        // Emitter specific
        /// <summary>
        /// This function creates a new Emitter node and attaches it to the specified parent node.
        /// </summary>
        /// <param name="parent">handle to parent node to which the new node will be attached</param>
        /// <param name="name">name of the node</param>
        /// <param name="matRes">handle to material resource used for rendering</param>
        /// <param name="effectRes">handle to effect resource used for configuring particle properties</param>
        /// <param name="maxParticleCount">maximal number of particles living at the same time</param>
        /// <param name="respawnCount">number of times a single particle is recreated after dying (-1 for infinite)</param>
        /// <returns>handle to the created node or 0 in case of failure</returns>
        public static int addEmitterNode(int parent, string name, int matRes, int effectRes, int maxParticleCount, int respawnCount)
        {
            if (parent < 0) throw new ArgumentOutOfRangeException("parent", Resources.UIntOutOfRangeExceptionString);
            if (name == null) throw new ArgumentNullException("name", Resources.StringNullExceptionString);
            if (matRes < 0) throw new ArgumentOutOfRangeException("matRes", Resources.UIntOutOfRangeExceptionString);
            if (effectRes < 0) throw new ArgumentOutOfRangeException("effectRes", Resources.UIntOutOfRangeExceptionString);
            if (maxParticleCount < 0) throw new ArgumentOutOfRangeException("maxParticleCount", Resources.UIntOutOfRangeExceptionString);

            return (int)NativeMethodsEngine.addEmitterNode((uint)parent, name, (uint)matRes, (uint)effectRes, (uint)maxParticleCount, respawnCount);
        }

        /// <summary>
        /// This function returns a specified property of the specified node. The specified node must be an Emitter node.
        /// </summary>
        /// <param name="node">handle to the node to be accessed</param>
        /// <param name="param">parameter to be accessed</param>
        /// <returns>value of the parameter</returns>
        public static float getEmitterParam(int node, EmitterNodeParams param)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.getEmitterParam((uint)node, param);
        }

        /// <summary>
        /// This function sets a specified property of the specified node to a specified value. The specified node must be an Emitter node, or alternatively a Group node. Calls on the latter node type are recursively passed to all Emitter child nodes.
        /// </summary>
        /// <param name="node">handle to the node to be modified</param>
        /// <param name="param">parameter to be modified</param>
        /// <param name="value">new value for the specified parameter</param>
        /// <returns>true in case of success otherwise false</returns>
        public static bool setEmitterParam(int node, EmitterNodeParams param, float value)
        {
            if (node < 0) throw new ArgumentOutOfRangeException("node", Resources.UIntOutOfRangeExceptionString);

            return NativeMethodsEngine.setEmitterParam((uint)node, param, value);
        }

        /// <summary>
        /// This function advances the simulation time of a particle system and continues the particle simulation with timeDelta being the time elapsed since the last call of this function.
        /// </summary>
        /// <param name="node">handle to the Emitter node which will be modified</param>
        /// <param name="timeDelta">time delta in seconds</param>
        /// <returns>true in case of success, otherwise false</returns>
        public static bool advanceEmitterTime(int node, float timeDelta)
        {
            return NativeMethodsEngine.advanceEmitterTime((uint)node, timeDelta);
        }

    }

    /// <summary>
    /// This Exception is thrown in case you don't use the correct Horde3D engine library version.
    /// </summary>
    [Serializable]
    public class LibraryIncompatibleException : Exception
    {
        public LibraryIncompatibleException()
        {
        }

        public LibraryIncompatibleException(string message)
            : base(message)
        {
        }

        public LibraryIncompatibleException(string message, Exception e)
            : base(message,e)
        {
        }

        protected LibraryIncompatibleException(SerializationInfo info, StreamingContext context) : base(info,context)
        {
        }

    }

}