//
//  UnityAR.m
//  Unity-iPhone
//
//  Created by Kimberly Spreen on 4/7/09.
//  Copyright 2009 Georgia Tech. All rights reserved.
//

#import <objc/runtime.h>
#import <sys/time.h>

#import "UnityAR.h"


#define VID_WIDTH  256
#define VID_HEIGHT 384

#define ORIG_VID_WIDTH  320
#define ORIG_VID_HEIGHT 426

extern "C"
int VIDEO_getAvailablePropertyStrings(int& count, void* str)
{
	count = 0;
	str = NULL;
	return 0;
}

extern "C"
int VIDEO_openVideo(const char*,int& handle)
{
	handle = 1;
	return 0;
}

extern "C"
int VIDEO_startVideo(int handle)
{
	printf("IPhone Camera Initialized\n");
	
	camWindow = [[UIWindow alloc] initWithFrame: [[UIScreen mainScreen] bounds]];
	camView = [[PLCameraView alloc] initWithFrame: [camWindow bounds]];
	[camWindow addSubview: camView];
	
	[camWindow makeKeyAndVisible];
	camWindow.windowLevel = -1;
	
	camController = [PLCameraController sharedInstance];
	[camController setDelegate: camView];
	[camController startPreview];
	[camController setDontShowFocus: YES];
	
	previewRect = CGRectMake((int) ((ORIG_VID_WIDTH - VID_WIDTH) / 2), (int) ((ORIG_VID_HEIGHT - VID_HEIGHT) / 2), VID_WIDTH, VID_HEIGHT);	
	return 0;
}

extern "C"
int VIDEO_getWidth (int handle, int& width) {
	width = VID_WIDTH;
	return 0;
}

extern "C"
int VIDEO_getHeight (int handle, int& height) {
	height = VID_HEIGHT;
	return 0;
}

extern "C"
int VIDEO_getDepth (int handle, int& val) {
	val = 32;
	return 0;
}

extern "C"
int VIDEO_getPixelFormat (int handle, int& val) {
	val = 32993;
	return 0;
}

extern "C"
int VIDEO_getFrame (int handle, unsigned char** dst, void* ts) {	
	if (camSurface)
		CFRelease(camSurface);
	camSurface = [camWindow createIOSurfaceWithFrame: previewRect];
	
	CoreSurfaceBufferLock(camSurface, 3);
	*dst = (unsigned char*) CoreSurfaceBufferGetBaseAddress(camSurface);
	return 0;
}

extern "C"
int VIDEO_releaseFrame (int handle) {	
	CoreSurfaceBufferUnlock(camSurface);
	return 0;
}

extern "C"
int VIDEO_stopVideo (int handle) {	
	CoreSurfaceBufferUnlock(camSurface);
	return 0;
}

extern "C"
int VIDEO_close (int handle) {	
	if (camSurface)
		CFRelease(camSurface);
	return 0;
}

extern "C"
void VIDEO_updateTextureByID(unsigned char* src, int texId) {
	
	GLint texture, textureStage, textureMode, textureFilter;
		
	// save opengl state
	glGetIntegerv(GL_TEXTURE_BINDING_2D, &texture);
	//glGetIntegerv(GL_CLIENT_ACTIVE_TEXTURE, &textureStage);
	//glGetTexEnviv(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, &textureMode);
	glGetTexParameteriv(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, &textureFilter);
	
	glBindTexture(GL_TEXTURE_2D, texId);
	glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, VID_WIDTH, VID_HEIGHT, GL_BGRA, GL_UNSIGNED_BYTE, src);
	
	// restore opengl state
	//glClientActiveTexture(textureStage);
	//glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, textureMode);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, textureFilter);
	glBindTexture(GL_TEXTURE_2D, texture);
}

extern "C"
void VIDEO_writeTextureToArray(void *src, void *dst, unsigned int width, unsigned int height, int format)
{
	// safeguard - array must be not null
	if( !src || !dst )
		return;
 
	// Color structure in VideoWrapper is four RGBA bytes
	unsigned char * img = (unsigned char*)(src);

	// Color structure in Unity is four RGBA floats
	float* color = (float*)(dst);

	if (format == 32993)
	{
		for (int y=0;y<height;y++)
		{
			for (int x=0;x<width;x++)
			{
				int offset =  (y * width + x) * 4;
				unsigned char* imgPixel = img + offset;
				float* pixel = color + offset;

				pixel[0] = (float)imgPixel[2]/255.0;
				pixel[1] = (float)imgPixel[1]/255.0;
				pixel[2] = (float)imgPixel[0]/255.0;
				pixel[3] = 1.0f;
			}
		}
	}
	else
	{
		for (int y=0;y<height;y++)
		{
			for (int x=0;x<width;x++)
			{
				int src_offset =  (y * width + x) * 3;
				int dst_offset =  (y * width + x) * 4;
				unsigned char* imgPixel = img + src_offset;
				float* pixel = color + dst_offset;
		
				pixel[0] = (float)imgPixel[0]/255.0;
				pixel[1] = (float)imgPixel[1]/255.0;
				pixel[2] = (float)imgPixel[2]/255.0;
				pixel[3] = 1.0f;
			}
		}
	}
	return;
}

