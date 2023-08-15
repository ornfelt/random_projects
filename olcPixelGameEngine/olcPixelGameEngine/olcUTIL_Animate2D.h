#pragma once

#include "olcPixelGameEngine.h"
#include "olcUTIL_Geometry2D.h"

namespace olc::utils::Animate2D
{
	// This class rerpresents a valid "frame" of an animation. It could be from any image source, and
	// any location withing that image source. Once it's constructed, it's advised not to change it, as
	// this likely indicates a usage bug.
	//
	// "Sourceless" frames are valid too - this is useful if you have a particular animation set, but 
	// want to apply it to a variety of sources, for example sprite maps with common layouts.
	class Frame
	{
	public:
		inline Frame(const olc::Renderable* gfxSource, const geom2d::rect<int32_t>& rectSource = { {0,0},{0,0} })
			: gfxImageSource(gfxSource), rectFrameSource(rectSource)
		{
			// If no source rectangle specified then use whole image source. Ignore in the event 
			// that a frame is set up as source-less
			if(gfxSource && rectFrameSource.size.x == 0)
				rectFrameSource.size = gfxSource->Sprite()->Size();
		}

		inline const olc::Renderable* GetSourceImage() const
		{
			return gfxImageSource;
		}

		inline const geom2d::rect<int32_t>& GetSourceRect() const
		{
			return rectFrameSource;
		}

	private:
		const olc::Renderable* gfxImageSource;
		geom2d::rect<int32_t> rectFrameSource;
	};

	// Animation styles decide how the frames should be traversed in time
	enum class Style : uint8_t
	{
		Repeat,		// Cycle through, go back to beginning
		OneShot,	// Play once and suspend on final frame
		PingPong,   // Traverse through forwards, then backwards
		Reverse,    // Cycle through sequence backwards
	};

	class FrameSequence
	{
	public:
		// Constructs a sequence of frames with a duration and a traversal style
		inline FrameSequence(const float fFrameDuration = 0.1f, const Style nStyle = Style::Repeat)
		{
			m_fFrameDuration = fFrameDuration;
			m_fFrameRate = 1.0f / m_fFrameDuration;
			m_nStyle = nStyle;
		}

		// Adds a frame to this sequence
		inline void AddFrame(const Frame& frame)
		{
			m_vFrames.emplace_back(frame);
		}

		// Returns a Frame Object for a given time into an animation
		inline const Frame& GetFrame(const float fTime) const
		{
			return m_vFrames[ConvertTimeToFrame(fTime)];
		}
		
	private:
		Style m_nStyle;
		std::vector<Frame> m_vFrames;
		float m_fFrameDuration = 0.1f;
		float m_fFrameRate = 10.0f;

		inline const size_t ConvertTimeToFrame(const float fTime) const
		{
			switch (m_nStyle)
			{
			case Style::Repeat:
				return size_t(fTime * m_fFrameRate) % m_vFrames.size();
				break;
			case Style::OneShot:
				return std::clamp(size_t(fTime * m_fFrameRate), size_t(0), m_vFrames.size() - 1);
				break;
			case Style::PingPong:
				// TODO
				break;
			case Style::Reverse:
				return (m_vFrames.size() - 1) - (size_t(fTime * m_fFrameRate) % m_vFrames.size());
				break;
			}

			return 0;
		}
	};

	// A Animate2D::State is a lightweight token that can be attached to things
	// that are animated. Under normal circumstances, it is never updated manually
	struct AnimationState
	{
	private:
		size_t nIndex = 0;
		float fTime = 0.0f;		
		template<typename StatesEnum>
		friend class Animation;
	};

	// Animation object holds a group of frame sequences and can mutate an AnimationState token
	template<typename StatesEnum>
	class Animation
	{
	public:
		Animation() = default;
		
		// Change an animation state token to a new state
		inline bool ChangeState(AnimationState& state, const StatesEnum& sStateName) const
		{
			size_t idx = m_mapStateIndices.at(sStateName);
			if (state.nIndex != idx)
			{
				state.fTime = 0.0f;
				state.nIndex = idx;
				return true;
			}

			return false;
		}

		// Update an animation state token
		inline void UpdateState(AnimationState& state, const float fElapsedTime) const
		{
			state.fTime += fElapsedTime;
		}

	public:
		// Retrieve the frame information for a given animation state
		inline const Frame& GetFrame(const AnimationState& state) const
		{
			return m_vSequences[state.nIndex].GetFrame(state.fTime);
		}

	public:
		// Add a named Frame sequence as a state
		inline void AddState(const StatesEnum& sStateName, const FrameSequence& sequence)
		{
			m_vSequences.emplace_back(sequence);
			m_mapStateIndices[sStateName] = m_vSequences.size() - 1;
		}

	private:
		std::vector<FrameSequence> m_vSequences;
		std::unordered_map<StatesEnum, size_t> m_mapStateIndices;
	};
}