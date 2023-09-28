<script lang="ts">

  let dark = "#031926"
  let light = "#fff"
  let primary = "#499f68"
  let secondary = "#FB9f89"

  // Spacing of ticks, in degrees
  let spacing = 10;

  let currentDial = 100;

</script>

<svg version="1.1"
      width="100" height="100"
     xmlns="http://www.w3.org/2000/svg"
     viewBox="-100 -100 100 100"
     >

  <circle cx="0" cy="0" r="80" fill={dark} />
  <circle cx="0" cy="0" r="70" fill={light} />

  {#each Array(Math.floor(270 / spacing) + 1) as _, index (index)} 
      <line x1="50" x2="70" y1="0" y2="0" stroke={primary} stroke-width="5" transform={`rotate(${-(index ) * spacing + 45}, 0, 0)`}/>
  {/each}

  <rect x="0" y="0" width="10" height="40" rx="1"  stroke={dark} stroke-width="5" transform={`rotate(${currentDial + 45}, 0, 0)`}/>

</svg>

