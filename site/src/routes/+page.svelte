<script lang="ts">
  import "../app.css"

  let dark = "#031926"
  let light = "#e7c9a9"
  let text = "#FFF"
  let primary = "#499f68"
  let primary2 = "#77b28c"
  let secondary = "#FB9f89"

  // Spacing of ticks, in degrees
  let spacing = 10;

  let currentHope = 0;
  $: currentDial = currentHope / 100 * 270;

  async function updateCurrentHope(){
    let req = await fetch("/hope"); 
    currentHope = await req.json();
  }

  async function updateCurrentHopeLoop(){
    updateCurrentHope();
    setTimeout(updateCurrentHopeLoop, 500)
  }

  updateCurrentHopeLoop()
</script>

<div class="flex flex-row justify-center items-center w-full h-40" style={`background-color: ${light}`}>
  <h1 class="text-white border-b-4 border-white">World Hope</h1>
</div>
<div class="flex flex-row justify-center items-center w-full h-80" style={`background-color: ${light}`}>
  <svg version="1.1"
       xmlns="http://www.w3.org/2000/svg"
       viewBox="-100 -100 200 200"
       class="aspect-square w-96"
       >

    <foreignObject x="-80" y="-80" width="160" height="160" >
        <div class="rotate-180" style={`background: conic-gradient(${primary}, ${primary2}); width: 100%; height: 100%`} xmlns="http://www.w3.org/1999/xhtml" />
    </foreignObject>
  
    <circle cx="0" cy="0" r="60" fill={light} />
   <circle cx="0" cy="0" r="100" stroke={light} stroke-width="50" fill="none" />
  
    {#each Array(Math.floor(270 / spacing) + 1) as _, index (index)} 
        <line x1="50" x2="70" y1="0" y2="0" stroke={light} stroke-width="5" transform={`rotate(${-(index ) * spacing + 45}, 0, 0)`}/>
    {/each}
    <rect class="transition-all" x="-5" y="-5" width="10" height="40" rx="1" fill={dark} stroke-width="5" transform={`rotate(${currentDial + 45}, 0, 0)`}/>
  
  </svg>
</div>


<div class="flex flex-row justify-center pt-10">
  <div class="w-full h-80" style="width: clamp(50%, 768px, 90%)">
    <h2>What is hope?</h2>
    <p>Hope is one of the most important factors to human happiness.</p>
    <p>Hope not only helps us believe the future will be better place, but motivates us to help make it so.</p>
    <p>Here, we've taken a shot at the impossible: measuring the world's hope, in realtime. It is a kind of "hope-o'-meter", if you will.</p>
  
    <h2>How is it done?</h2>
    <p>Our methodology, as it stands now, is admittadly somewhat crude. At a high level, it breaks down into three simple steps:</p>
    <ul class="list-disc pl-10">
      <li>Scrape a variety of reputable (and unreputable) news organization's websites (Reuters, Associated Press, etc.).</li>
      <li>Perform sentimant analysis using <a href="https://doi.org/10.1609/icwsm.v8i1.14550">VADER</a>. We sum the "positive" and "negative" outputs.</li>
      <li>Sum the result over all sources, then offset the result to be relative to all recorded history.</li>
    </ul>
  
  <h2>Questions?</h2>
  <p>This site is a project by <a href="https://elijahpotter.dev">Elijah Potter</a> for a class at the Colorado School of Mines.</p>
  </div>
</div>
