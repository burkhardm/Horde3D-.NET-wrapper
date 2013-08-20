// *************************************************************************************************
//
// Knight .NET - sample application for Horde3D .NET wrapper
// ----------------------------------------------------------
//
// Copyright (C) 2007 Nicolas Schulz and Martin Burkhard
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

namespace Horde3DNET.Samples.KnightNET
{
    internal class Application
    {
	    private float			_x, _y, _z, _rx, _ry;	// Viewer position and orientation
	    private float			_velocity;				// Velocity for movement
        private float           _curFPS, _timer;
        private string          _fpsText;

        private bool            _freeze, _showFPS, _debugViewMode;
        private float           _animTime, _weight;
    	
	    // Engine objects
	    private int	            _fontMatRes, _logoMatRes;
        private int             _knight, _particleSys;

        // workaround
        private bool            _initialized = false;

    	
        // Convert from degrees to radians
        public static float degToRad( float f ) 
        {
	        return f * (3.1415926f / 180.0f);
        }

	    public Application()
        {
            _x = 5; _y = 3; _z = 19; _rx = 7; _ry = 15; _velocity = 10.0f;
            _curFPS = 30; _timer = 0;

            _freeze = false; _showFPS = false; _debugViewMode = false;
            _animTime = 0; _weight = 1.0f;
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
            if (!Horde3D.loadPipelineConfig("pipeline_Knight.xml"))
            {
                Horde3DUtils.dumpMessages();
                return false;
            }

	        // Set options
	        Horde3D.setOption( Horde3D.EngineOptions.LoadTextures, 1 );
	        Horde3D.setOption( Horde3D.EngineOptions.TexCompression, 0 );
            Horde3D.setOption( Horde3D.EngineOptions.FastAnimation, 0 );
	        Horde3D.setOption( Horde3D.EngineOptions.AnisotropyFactor, 8 );
	        Horde3D.setOption( Horde3D.EngineOptions.ShadowMapSize, 2048 );

            // Add resources
            // Font
            _fontMatRes = Horde3D.addResource(Horde3D.ResourceTypes.Material, "font.material.xml", 0);
            // Logo
            _logoMatRes = Horde3D.addResource(Horde3D.ResourceTypes.Material, "logo.material.xml", 0);
            // Environment
            int envRes = Horde3D.addResource(Horde3D.ResourceTypes.SceneGraph, "scene.scene.xml", 0);
	        // Knight
	        int knightRes = Horde3D.addResource( Horde3D.ResourceTypes.SceneGraph, "knight.scene.xml", 0 );
	        int knightAnim1Res = Horde3D.addResource( Horde3D.ResourceTypes.Animation, "knight_order.anim", 0 );
	        int knightAnim2Res = Horde3D.addResource( Horde3D.ResourceTypes.Animation, "knight_attack.anim", 0 );
	        // Particle system
            int particleSysRes = Horde3D.addResource(Horde3D.ResourceTypes.SceneGraph, "particleSys1.scene.xml", 0);


            // Load resources
            Horde3DUtils.loadResourcesFromDisk( "content" );


            // Add scene nodes
            // Add environment
            Horde3D.addNodes( Horde3D.RootNode, envRes );
	        
            // Add knight
            _knight = Horde3D.addNodes(Horde3D.RootNode, knightRes);
	        Horde3D.setNodeTransform( _knight, 0, 0, 0, 0, 180, 0, 0.1f, 0.1f, 0.1f );
	        Horde3D.setupModelAnimStage( _knight, 0, knightAnim1Res, string.Empty, false );
            Horde3D.setupModelAnimStage(_knight, 1, knightAnim2Res, string.Empty, false);

            // Attach particle system to hand joint
	        int hand = Horde3D.getNodeChild( _knight, "Bip01_R_Hand", 0, true );
	        _particleSys = Horde3D.addNodes( hand, particleSysRes );
            Horde3D.setNodeTransform(_particleSys, 0, 40, 0, 90, 0, 0, 1, 1, 1);

            // Add light source
            int light = Horde3D.addLightNode(Horde3D.RootNode, "Light1", 0, "LIGHTING", "SHADOWMAP");
            Horde3D.setNodeTransform(light, 0, 1, 15, 30, 0, 0, 1, 1, 1);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Radius, 30);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.FOV, 90);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowMapCount, 1);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.ShadowMapBias, 0.01f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_R, 1.0f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_G, 0.7f);
            Horde3D.setLightParam(light, Horde3D.LightNodeParams.Col_B, 0.7f);

	        // Customize post processing effects
	        int matRes = Horde3D.findResource( Horde3D.ResourceTypes.Material, "postHDR.material.xml" );
	        // hdrParams: exposure, brightpass threshold, brightpass offset
            Horde3D.setMaterialUniform(matRes, "hdrParams", 2.5f, 0.6f, 0.06f, 0);

            _initialized = true;

	        return true;
        }

        public void mainLoop(float fps)
        {
	        _curFPS = fps;
            _timer += 1 / fps;

            Horde3D.setOption( Horde3D.EngineOptions.DebugViewMode, _debugViewMode ? 1.0f : 0.0f );
        	
	        if( !_freeze )
	        {
		        _animTime += 1.0f / _curFPS;

		        // Do animation blending
		        Horde3D.setModelAnimParams( _knight, 0, _animTime * 24.0f, _weight );
		        Horde3D.setModelAnimParams( _knight, 1, _animTime * 24.0f, 1.0f - _weight );

		        // Animate particle system
		        Horde3D.advanceEmitterTime( _particleSys, 1.0f / _curFPS );
	        }

            // Set camera parameters
            Horde3D.setNodeTransform(Horde3D.PrimeTimeCam, _x, _y, _z, _rx, _ry, 0, 1, 1, 1);


            if (_showFPS)
            {
		        // Avoid updating FPS text every frame to make it readable
		        if( _timer > 0.3f )
		        {
                    _fpsText = string.Format("FPS: {0:F2}", fps);
			        _timer = 0;
		        }

                // Show text
                if( _fpsText != null)
                    Horde3DUtils.showText(_fpsText, 0, 0.95f, 0.03f, 0, _fontMatRes);

                string text = string.Format("Weight: {0:F2}", _weight);
                Horde3DUtils.showText(text, 0, 0.91f, 0.03f, 0, _fontMatRes);
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
            Horde3D.setupCameraView(Horde3D.PrimeTimeCam, 45.0f, (float)width / height, 0.1f, 1000.0f);
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
                case Keys.F5:

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

            if (InputManager.IsKeyDown(Keys.D1))	// 1
            {
                _weight += 2 / _curFPS;
                if (_weight > 1) _weight = 1;
            }
            if (InputManager.IsKeyDown(Keys.D2))	// 2
            {
                _weight -= 2 / _curFPS;
                if (_weight < 0) _weight = 0;
            }

        }

        public void mouseHandler(float dX, float dY)
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
