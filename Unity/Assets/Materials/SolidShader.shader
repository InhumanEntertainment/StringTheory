Shader "Inhuman/Solid Color" 
{
    Properties 
	{
        _Color ("Color", Color) = (1, 1, 1,1)
    }
    SubShader 
	{
        Pass 
		{
			Lighting Off
            Color [_Color]
        }
    }
} 

