// *************************************************************************************************
//
// Chicago .NET - sample application for Horde3D .NET wrapper
// ----------------------------------------------------------
//
// Copyright (C) 2006-07 Nicolas Schulz and Martin Burkhard
//
// This file is intended for use as a code example, and may be used, modified, 
// or distributed in source or object code form, without restriction. 
// This sample is not covered by the LGPL.
//
// The code and information is provided "as-is" without warranty of any kind, 
// either expressed or implied.
//
// *************************************************************************************************

using System;
using System.Windows.Forms;
using Horde3DNET;

namespace Horde3DNET.Samples.ChicagoNET
{
    internal class Application
    {
	    private float			_x, _y, _z, _rx, _ry;	// Viewer position and orientation
	    private float			_velocity;				// Velocity for movement
	    private float			_curFPS, _timer;
        private string          _fpsText;

        private bool            _freeze, _showFPS, _debugViewMode;

	    private CrowdSim		_crowdSim;
    	
	    // Engine objects
	    private int	            _fontMatRes, _logoMatRes;

        // workaround
        private bool _initialized = false;

    	
        // Convert from degrees to radians
        public static float degToRad( float f ) 
        {
	        return f * (3.1415926f / 180.0f);
        }

	    public Application()
        {
	        _x = 0; _y = 2; _z = 0; _rx = 0; _ry = 0; _velocity = 10.0f;
            _curFPS = 0; _timer = 0.0f;
            _freeze = false; _showFPS = false; _debugViewMode = false;
            _fpsText = string.Empty;
        }

        public bool init()
        {
	        // Initialize engine
            if (!Horde3D.init())
            {
                Horde3DUtils.dumpMessages();
                return false;
            }

            // Set paths for resources
            Horde3D.setResourcePath(Horde3D.ResourceTypes.SceneGraph, "models");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Geometry, "models");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Animation, "models");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Material, "materials");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Code, "shaders");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Shader, "shaders");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.Texture2D, "textures");
            Horde3D.setResourcePath(Horde3D.ResourceTypes.TextureCube, "textures");
	        Horde3D.setResourcePath( Horde3D.ResourceTypes.Effect, "effects" );

	        // Load pipeline configuration
	        if( !Horde3D.loadPipelineConfig( "pipeline_Chicago.xml" ) )
	        {
                Horde3DUtils.dumpMessages();
	            return false;
	        }

	        // Set options
	        Horde3D.setOption( Horde3D.EngineOptions.LoadTextures, 1 );
	        Horde3D.setOption( Horde3D.EngineOptions.TexCompression, 0 );
	        Horde3D.setOption( Horde3D.EngineOptions.AnisotropyFactor, 8 );
	        Horde3D.setOption( Horde3D.EngineOptions.ShadowMapSize, 2048 );
            Horde3D.setOption( Horde3D.EngineOptions.FastAnimation, 0 );

            // Add resources
            // Font
            _fontMatRes = Horde3D.addResource(Horde3D.ResourceTypes.Material, "font.material.xml", 0);
            // Logo
            _logoMatRes = Horde3D.addResource(Horde3D.ResourceTypes.Material, "logo.material.xml", 0);
            // Shader for deferred shading
            int lightMatRes = Horde3D.addResource(Horde3D.ResourceTypes.Material, "light.material.xml", 0);
            // Environment
            int envRes = Horde3D.addResource(Horde3D.ResourceTypes.SceneGraph, "scene.scene.xml", 0);
            // Skybox
            //int skyBoxRes = Horde3D.addResource(Horde3D.ResourceTypes.SceneGraph, "skybox.scene.xml", 0);


            // Load resources
            Horde3DUtils.loadResourcesFromDisk( "content" );


            // Add scene nodes
            // Add environment
            Horde3D.addNodes( Horde3D.RootNode, envRes );
            // Add skybox
            //int sky = Horde3D.addNodes( Horde3D.RootNode, skyBoxRes );
            //Horde3D.setNodeTransform(sky, 0, 0, 0, 0, 0, 0, 210, 50, 210);
            // Add light source
            int light = Horde3D.addLightNode(Horde3D.RootNode, "Light1", lightMatRes, "LIGHTING", "SHADOWMAP");
            Horde3D.setNodeTransform(light, 0, 25, -25, -120, 0, 0, 1, 1, 1);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Radius, 100);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.FOV, 90);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowMapCount, 3);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowSplitLambda, 0.95f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_R, 0.98f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_G, 0.6f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_B, 0.5f);

            // Add light source
            light = Horde3D.addLightNode(Horde3D.RootNode, "Light2", lightMatRes, "LIGHTING", "SHADOWMAP");
            Horde3D.setNodeTransform(light, 0, 25, 30, -60, 0, 0, 1, 1, 1);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Radius, 50);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.FOV, 90);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowMapCount, 3);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowSplitLambda, 0.95f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_R, 0.5f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_G, 0.6f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_B, 0.98f);

            _crowdSim = new CrowdSim();        
	        _crowdSim.init();

            _initialized = true;

	        return true;
        }

        public void mainLoop(float fps)
        {
	        _curFPS = fps;
            _timer += 1 / fps;

            Horde3D.setOption(Horde3D.EngineOptions.DebugViewMode, _debugViewMode ? 1.0f : 0.0f);
        	
	        if( !_freeze )
	        {
		        _crowdSim.update( _curFPS );
	        }

            // Set camera parameters
            Horde3D.setNodeTransform(Horde3D.PrimeTimeCam, _x, _y, _z, _rx, _ry, 0, 1, 1, 1);


            if (_showFPS)
            {
                // Avoid updating FPS text every frame to make it readable
                if (_timer > 0.3f)
                {
                    _fpsText = string.Format("FPS: {0:F2}", fps);
                    _timer = 0;
                }

                // Show text
                if (_fpsText != null)
                    Horde3DUtils.showText(_fpsText, 0, 0.95f, 0.03f, 0, _fontMatRes);
            }

            // Show logo
            Horde3D.showOverlay(0.75f, 0, 0, 0, 1, 0, 1, 0,
                                1, 0.2f, 1, 1, 0.75f, 0.2f, 0, 1,
                                7, _logoMatRes);


            // Render scene
            Horde3D.render();

            // Write all messages to log file
            Horde3DUtils.dumpMessages();
        }

        public void release()
        {      	
	        // Release engine
	        Horde3D.release();
        }

        public void resize(int width, int height)
        {
            if (!_initialized) return;

            // Resize viewport
            Horde3D.resize( 0, 0, width, height );

            // Set virtual camera parameters
            Horde3D.setupCameraView(Horde3D.PrimeTimeCam, 45.0f, (float) width/height, 0.1f, 1000.0f);
        }

        public void keyPressEvent(Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    _freeze = !_freeze;
                    break;

                case Keys.F7:
                    _debugViewMode = !_debugViewMode;
                    break;

                case Keys.F9:
                    _showFPS = !_showFPS;
                    break;
            }
        }

        public void keyHandler()
        {
            float curVel = _velocity / _curFPS;

            if(InputManager.IsKeyDown(Keys.W))
            {
                // Move forward
                _x -= (float)Math.Sin(degToRad(_ry)) * (float)Math.Cos(-degToRad(_rx)) * curVel;
                _y -= (float)Math.Sin(-degToRad(_rx)) * curVel;
                _z -= (float)Math.Cos(degToRad(_ry)) * (float)Math.Cos(-degToRad(_rx)) * curVel;
            }

            if (InputManager.IsKeyDown(Keys.S))
            {
                // Move backward
                _x += (float)Math.Sin(degToRad(_ry)) * (float)Math.Cos(-degToRad(_rx)) * curVel;
                _y += (float)Math.Sin(-degToRad(_rx)) * curVel;
                _z += (float)Math.Cos(degToRad(_ry)) * (float)Math.Cos(-degToRad(_rx)) * curVel;
            }

            if(InputManager.IsKeyDown(Keys.A))
            {                
                // Strafe left
                _x += (float)Math.Sin(degToRad(_ry - 90)) * curVel;
                _z += (float)Math.Cos(degToRad(_ry - 90)) * curVel;
            }

            if(InputManager.IsKeyDown(Keys.D))
            { 
                // Strafe right
                _x += (float)Math.Sin(degToRad(_ry + 90)) * curVel;
                _z += (float)Math.Cos(degToRad(_ry + 90)) * curVel;
            }
        }

        public void mouseMoveEvent(float dX, float dY)
        {
            // Look left/right
            _ry -= dX / 100 * 30;

            // Loop up/down but only in a limited range
            _rx += dY / 100 * 30;
            if (_rx > 90) _rx = 90;
            if (_rx < -90) _rx = -90;
        }

    }
}
