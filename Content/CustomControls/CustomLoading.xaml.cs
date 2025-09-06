namespace wwrc_maui.Content.CustomControls;

public partial class CustomLoading : Grid
{
    #region bindable properties
    public bool IsAnimate
    {
        get => (bool)GetValue(IsAnimateProperty);
        set { SetValue(IsAnimateProperty, value); }
    }
    public static BindableProperty IsAnimateProperty =
        BindableProperty.Create(nameof(IsAnimate), typeof(bool), typeof(CustomLoading),
            defaultValue: false, propertyChanged: (bindable, oldVal, newVal) =>
            { ((CustomLoading)bindable).UpdateIsAnimate((bool)newVal); });
    public void UpdateIsAnimate(bool data)
    {
        if (data) StartAnimation();
        else StopAnimation();
    }
    #endregion

    public Action<bool>? BgTapAction;

    public CustomLoading()
    {
        InitializeComponent();
        IsVisible = false;
    }

    public void OnBg_Tapped(object sender, EventArgs e)
    { BgTapAction?.Invoke(true); }

    public void StartAnimation()
    {
        //var bgAnm = new Animation();
        //var mainAnim = new Animation();
        //var bgFadeIn = new Animation(v => stack_bg.Opacity = v, 0, 0.7, Easing.SinIn);
        //var blueBgFadeIn = new Animation(v => img_bluebg.Opacity = v, 0, 0.4, Easing.Linear);
        //var containerScale1 = new Animation(v => img_container.Scale = v, 1, 0.9, Easing.Linear);
        //var containerScale2 = new Animation(v => img_container.Scale = v, 0.9, 1, Easing.Linear);
        //var circling1Rotate = new Animation(v => img_circling1.RotationX = v, 0, 180, Easing.Linear);
        //var circling1FadeIn = new Animation(v => img_circling1.Opacity = v, 0, 0.7, Easing.SinIn);
        //var circling1FadeOut = new Animation(v => img_circling1.Opacity = v, 0.7, 0, Easing.SinOut);
        //var circling1clock = new Animation(v => img_circling1.Rotation = v, 0, 360, Easing.Linear);
        //var circling2Rotate = new Animation(v => img_circling2.RotationY = v, 0, 360, Easing.CubicInOut);
        //var circling2FadeIn = new Animation(v => img_circling2.Opacity = v, 0, 1, Easing.SinIn);
        //var circling2FadeOut = new Animation(v => img_circling2.Opacity = v, 1, 0, Easing.SinOut);

        //bgAnm.Add(0, 0.5, bgFadeIn);
        //bgAnm.Add(0, 0.8, blueBgFadeIn);
        //mainAnim.Add(0, 0.3, containerScale1);
        //mainAnim.Add(0.3, 1, containerScale2);
        //mainAnim.Add(0, 1, circling1Rotate);
        //mainAnim.Add(0, 0.3, circling1FadeIn);
        //mainAnim.Add(0.7, 1, circling1FadeOut);
        //mainAnim.Add(0, 1, circling1clock);
        //mainAnim.Add(0, 1, circling2Rotate);
        //mainAnim.Add(0, 0.3, circling2FadeIn);
        //mainAnim.Add(0.7, 1, circling2FadeOut);

        //IsVisible = true;
        //bgAnm.Commit(this, "BgAnimation", 16, 500, null, (v, c) => { }, () => false);
        //mainAnim.Commit(this, "MainAnimation", 16, 1200, null, (v, c) => { }, () => true);
    }

    public void StopAnimation()
    {
        //IsVisible = false;
        //this.AbortAnimation("BgAnimation");
        //this.AbortAnimation("MainAnimation");
    }
}