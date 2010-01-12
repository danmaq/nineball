package danmaq.nineball.state
{
	
	import danmaq.nineball.entity.IEntity;
	
	public interface IState
	{
		
		////////// METHODS //////////
		
		function setup( entity:IEntity ):void;
		
		function update( entity:IEntity ):void;

		function teardown( entity:IEntity ):void;
		
	}
}
