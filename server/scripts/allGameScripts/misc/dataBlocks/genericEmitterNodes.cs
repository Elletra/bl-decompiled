datablock ParticleEmitterNodeData (GenericEmitterNode)
{
	timeMultiple = 1;
};

datablock ParticleEmitterNodeData (HalfEmitterNode)
{
	timeMultiple = 1 / 2;
};

datablock ParticleEmitterNodeData (FifthEmitterNode)
{
	timeMultiple = 1 / 5;
};

datablock ParticleEmitterNodeData (TenthEmitterNode)
{
	timeMultiple = 1 / 10;
};

datablock ParticleEmitterNodeData (TwentiethEmitterNode)
{
	timeMultiple = 1 / 20;
};

datablock ParticleEmitterNodeData (FourtiethEmitterNode)
{
	timeMultiple = 1 / 40;
};


function ParticleEmitterNode::onRemove ( %obj )
{
	// Your code here
}
