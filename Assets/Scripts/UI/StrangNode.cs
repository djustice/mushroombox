using Unity.VisualScripting;

[UnitTitle("Strang")]
[TypeIcon(typeof(string))]
public class StrangNode : Unit
{
	[DoNotSerialize, PortLabelHidden]
	public ValueInput strIn;
	[DoNotSerialize, PortLabelHidden]
	public ValueOutput strOut;
	
	protected override void Definition()
	{
		strIn = ValueInput<string>("", "            ");
		strOut = ValueOutput<string>("", (flow) => { return flow.GetValue<string>(strIn).TrimEnd(); });
	}
}