#include "ParticleEffect.h"
#include "DebugMemory.h"

ParticleEffect::ParticleEffect(int effectsType , Vector2D origin  , int numParticles , float maximumRange , float maximumLifetime, Core::RGB color )
{
	effectType = effectsType;
	numberParticles = numParticles;
	particles = new Particle[numberParticles];
	startPos = origin;
	anyAlive = true;
	for(int i=0; i<numberParticles; i++)
	{
		particles[i].position = origin;
		particles[i].size = random.randomInRange(2.0f, 3.0f);
		particles[i].size = (particles[i].size < 2) ? 2.0f : particles[i].size;
		if(effectType == 1)
		{
			particles[i].originalVelocity = random.randomUnitVector()*random.randomInRange(maximumRange, 10.0f);
			particles[i].color = randColor.varyColor(color, 40);
			particles[i].lifetime = random.randomInRange(.5f,maximumLifetime);
			particles[i].totalLifetime = particles[i].lifetime;
		}
		else if(effectType== 2)
		{
			particles[i].lifetime = random.randomInRange(.15f, .25f);
			particles[i].totalLifetime = particles[i].lifetime;
			particles[i].color = randColor.varyColor(color, 40);
			particles[i].originalVelocity.y = random.randomInRange(150.0f, 175.0f);
			particles[i].originalVelocity.x = random.randomInRange(-10.0f, 10.0f);
		}
		else if(effectType==3)
		{
			particles[i].lifetime = random.randomInRange(1.0f, 2.0f);
			particles[i].color = randColor.varyColor(color, 35);
			particles[i].totalLifetime = particles[i].lifetime;
			if(i<(numberParticles/2))
			{
				particles[i].originalVelocity.x = random.randomInRange(-100.0f, 100.0f);
				particles[i].position.y += random.randomInRange(-15.0f, 15.0f);
			}
			else
			{
				particles[i].originalVelocity.y = random.randomInRange(-100.0f, 100.0f);
				particles[i].position.x += random.randomInRange(-15.0f, 15.0f);
			}
		}
	}
}

ParticleEffect::~ParticleEffect()
{
	delete [] particles;
}

void ParticleEffect::setNewOrigin(Vector2D newOrigin)
{
	for(int i=0; i<numberParticles; i++)
	{
		particles[i].position = newOrigin;
	}
}

void ParticleEffect::Update(float dt)
{
	if(anyAlive)
	{
		for(int i=0; i<numberParticles; i++)
		{
			anyAlive = false;
			if(particles[i].lifetime > 0)
			{
				anyAlive = true;
				if(effectType == 1||effectType == 3)
					particles[i].position = particles[i].position + particles[i].originalVelocity*dt;
				else
					particles[i].position = particles[i].position + particles[i].newVelocity*dt;
				particles[i].lifetime -= dt;
			}
		}
	}
}

void ParticleEffect::CheckandCreateNewParticles(Vector2D position, float angle)
{
	for(int i=0; i<numberParticles; i++)
	{
		if(particles[i].lifetime <= 0)
		{
			particles[i].position = position;
			particles[i].newVelocity = Rotate(angle) * particles[i].originalVelocity ;
			particles[i].lifetime = random.randomInRange(.15f, .25f);
			particles[i].totalLifetime = particles[i].lifetime;
		}
	}
}

void ParticleEffect::Draw(Core::Graphics& graphics)
{
	if(anyAlive)
	{
		Vector2D pointA, pointB, pointC, pointD, pointE, pointF;
		for(int i=0; i<numberParticles; i++)
		{
			if(particles[i].lifetime > 0)
			{
				Core::RGB particleColor = randColor.brightness(particles[i].color, particles[i].lifetime/particles[i].totalLifetime);
				graphics.SetColor(particleColor);
				Vector2D bolt[] = { 
					Vector2D(particles[i].position.x + particles[i].size/2 , particles[i].position.y - particles[i].size),
					Vector2D(particles[i].position.x + particles[i].size, particles[i].position.y),
					Vector2D(particles[i].position.x + particles[i].size/2, particles[i].position.y+particles[i].size),
					Vector2D(particles[i].position.x - particles[i].size/2, particles[i].position.y + particles[i].size),
					Vector2D(particles[i].position.x - particles[i].size, particles[i].position.y ),
				Vector2D(particles[i].position.x - particles[i].size/2, particles[i].position.y - particles[i].size)};
				for(int b=0; b<6; b++)
				{
					Vector2D start = bolt[b] ;
					Vector2D end = bolt[(b+1)%6];
					graphics.DrawLine(start.x, start.y, end.x, end.y);
				}
			}
		}
	}
}