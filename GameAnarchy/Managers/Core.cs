namespace GameAnarchy;

public partial class Manager : SingletonManager<Manager> {
    public override bool IsInit { get; set; }

    public override void DeInit() { }

    public override void Init() { }
}
